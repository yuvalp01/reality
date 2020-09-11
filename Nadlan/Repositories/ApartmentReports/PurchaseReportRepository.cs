using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
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
            var allPurchase = GetAllPurchase(apartmentId);

            var investment = GetInvestment(allPurchase);
            var totalCost = GetTotalCost(allPurchase);
            var renovationCost = allPurchase.Where(purchaseFilters.GetRenovationFilter());
            var expensesNoRenovation = allPurchase.Where(purchaseFilters.GetCostNotRenovataionFilter());
            var accountSummary = GetAccountSummaryPurchase(allPurchase);

            PurchaseReport purchaseReport = new PurchaseReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                TotalCost = await Task.FromResult(totalCost.Sum(a => a.Amount)),
                RenovationCost = await Task.FromResult(renovationCost.Sum(a => a.Amount)),
                ExpensesNoRenovation = await Task.FromResult(expensesNoRenovation.Sum(a => a.Amount)),
                AccountsSum = await Task.FromResult(accountSummary.ToList())
            };

            purchaseReport.AccountsSum = KeepRenovationAccountsTogether(purchaseReport.AccountsSum);

            return purchaseReport;
        }

        private IEnumerable<AccountSummary> GetAccountSummaryPurchase(IEnumerable<Transaction> transactions)
        {
            var basic = purchaseFilters.GetTotalCostFilter();
            return transactions
                .Where(basic)
                .GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = Math.Abs(a.Sum(s => s.Amount))
                });
        }




        private List<AccountSummary> KeepRenovationAccountsTogether(IEnumerable<AccountSummary> accnoutsList_)
        {
            var accnoutsList = accnoutsList_.ToList();
            var contractorIndex = accnoutsList.FindIndex(a => a.AccountId == 17);
            var miscellaneousIndex = accnoutsList.FindIndex(a => a.AccountId == 6);

            if (contractorIndex == -1 || miscellaneousIndex == -1)
            {
                return accnoutsList;
            }
            var miscellaneousRef = accnoutsList.Find(a => a.AccountId == 6);
            var contractorRef = accnoutsList.Find(a => a.AccountId == 17);
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
