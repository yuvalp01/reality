using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
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
            var predicate = GetRegularTransactionsFilter(apartmentId, 0, true);

            // var allPurchase = GetAllPurchase(apartmentId);

            //var investment = GetInvestment(allPurchase);
            // var investment = GetInvestment(apartmentId);
            // var totalCost = GetTotalCost(allPurchase);
            //var totalCost = GetTotalCost(apartmentId);
            //  var renovationCost = Context.Transactions.Where(purchaseFilters.GetRenovationFilter()).Sum(a=>a.Amount);
            //var expensesNoRenovation = Context.Transactions.Where(purchaseFilters.GetCostNotRenovataionFilter());
            // var accountSummary = GetAccountSummaryPurchase(apartmentId);

            PurchaseReport purchaseReport = new PurchaseReport
            {
                Investment = GetAccountSum(apartmentId, 13),
                TotalCost = GetTotalCost(apartmentId),
                RenovationCost = GetRenovationCost(apartmentId),
                ExpensesNoRenovation = GetExpensesWithoutRenovaiton(apartmentId),
                AccountsSum = GetAccountSummaryPurchase(apartmentId)
            };

            purchaseReport.AccountsSum = await Task.FromResult(KeepRenovationAccountsTogether(purchaseReport.AccountsSum));

            return purchaseReport;
        }


        private decimal GetRenovationCost(int apartmentId)
        {
            var basic = GetRegularTransactionsFilter(apartmentId, 0, true);
            var purchaceFilter = purchaseFilters.GetTotalCostFilter();

            return Context.Transactions
               .Include(a => a.Account)
               .Where(basic)
               .Where(purchaceFilter)
               .Where(a => a.AccountId == 6 || a.AccountId == 17)
               .Sum(a => a.Amount);
        }

        private decimal GetExpensesWithoutRenovaiton(int apartmentId)
        {
            var basic = GetRegularTransactionsFilter(apartmentId, 0, true);
            var purchaceFilter = purchaseFilters.GetTotalCostFilter();



            return Context.Transactions
               .Include(a => a.Account)
               .Where(basic)
               .Where(purchaceFilter)
               .Where(a => !(a.AccountId == 6 || a.AccountId == 17 || a.AccountId == 12))
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


        //private IEnumerable<AccountSummary> GetAccountSummaryPurchase(IEnumerable<Transaction> transactions)
        //{
        //    var basic = purchaseFilters.GetTotalCostFilter();
        //    return transactions
        //        .Where(basic)
        //        .GroupBy(g => new { g.AccountId, g.Account.Name })
        //        .OrderBy(a => a.Sum(s => s.Amount))
        //        .Select(a => new AccountSummary
        //        {
        //            AccountId = a.Key.AccountId,
        //            Name = a.Key.Name,
        //            Total = Math.Abs(a.Sum(s => s.Amount))
        //        });
        //}




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
