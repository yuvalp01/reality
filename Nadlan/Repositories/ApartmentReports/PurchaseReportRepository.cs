using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models.Enums;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.ApartmentReports
{
    public class PurchaseReportRepository : ApartmentReportRepository
    {
        private PurchaseFilters purchaseFilters = new PurchaseFilters();

        public PurchaseReportRepository(NadlanConext conext) : base(conext)
        {
        }


        public async Task<PurchaseReport> GetPurchaseReport(int apartmentId)
        {

            PurchaseReport purchaseReport = new PurchaseReport();
            purchaseReport.Investment = GetAccountSum(apartmentId, 13);
            purchaseReport.TotalCost = GetTotalCost(apartmentId);
            purchaseReport.RenovationCost = GetRenovationCost(apartmentId);
            purchaseReport.ExpensesNoRenovation = GetExpensesWithoutRenovaiton(apartmentId);
            purchaseReport.AccountsSum = GetAccountSummaryPurchase(apartmentId);

            purchaseReport.AccountsSum = await Task.FromResult(KeepRenovationAccountsTogether(purchaseReport.AccountsSum));

            return purchaseReport;
        }


        private decimal GetRenovationCost(int apartmentId)
        {
            var basic = GetRegularTransactionsFilter(apartmentId, 0, true);
            var purchaceFilter = purchaseFilters.GetTotalCostFilter();

            return Context.Transactions
               //.Include(a => a.Account)
               .Where(basic)
               .Where(purchaceFilter)
               .Where(a => a.AccountId == (int)Accounts.RenovationMiscellaneous ||
                           a.AccountId == (int)Accounts.RenovationContractor)
               .Sum(a => a.Amount);
        }

        private decimal GetExpensesWithoutRenovaiton(int apartmentId)
        {
            var basic = GetRegularTransactionsFilter(apartmentId, 0, true);
            var purchaceFilter = purchaseFilters.GetTotalCostFilter();

            return Context.Transactions
               .Where(basic)
               .Where(purchaceFilter)
               .Where(a => !(a.AccountId == (int)Accounts.RenovationMiscellaneous ||
                             a.AccountId == (int)Accounts.RenovationContractor ||
                             a.AccountId == (int)Accounts.ApartmentCost))
               .Sum(a => a.Amount);

        }


        protected List<AccountSummary> GetAccountSummaryPurchase(int apartmentId)
        {
            var basic = purchaseFilters.GetTotalCostFilter();
            var accountSummary = Context.Transactions
                .Include(a => a.Account)
                .Where(basic)
                .Where(a => a.ApartmentId == apartmentId)
                .GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = a.Sum(s => s.Amount)
                });
            return accountSummary.ToList();
        }


        private List<AccountSummary> KeepRenovationAccountsTogether(IEnumerable<AccountSummary> accnoutsList_)
        {
            var accnoutsList = accnoutsList_.ToList();
            var contractorIndex = accnoutsList.FindIndex(a => a.AccountId == (int)Accounts.RenovationContractor);
            var miscellaneousIndex = accnoutsList.FindIndex(a => a.AccountId == (int)Accounts.RenovationMiscellaneous);

            if (contractorIndex == -1 || miscellaneousIndex == -1)
            {
                return accnoutsList;
            }
            var miscellaneousRef = accnoutsList.Find(a => a.AccountId == (int)Accounts.RenovationMiscellaneous);
            var contractorRef = accnoutsList.Find(a => a.AccountId == (int)Accounts.RenovationContractor);
            var renovationContractor = new AccountSummary()
            {
                AccountId = accnoutsList[contractorIndex].AccountId,
                Name = accnoutsList[contractorIndex].Name,
                Total = accnoutsList[contractorIndex].Total
            };

            var renovationMiscellaneous = new AccountSummary()
            {
                AccountId = accnoutsList[miscellaneousIndex].AccountId,
                Name = accnoutsList[miscellaneousIndex].Name,
                Total = accnoutsList[miscellaneousIndex].Total
            };
            int min = Math.Min(contractorIndex, miscellaneousIndex);
            accnoutsList.Remove(miscellaneousRef);
            accnoutsList.Remove(contractorRef);
            accnoutsList.Insert(min, renovationContractor);
            accnoutsList.Insert(min + 1, renovationMiscellaneous);
            return accnoutsList;
        }


    }
}


