using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class ApartmentReportRepositoryNew
    {

        protected NadlanConext Context { get; set; }
        private PurchaseFilters purchaseFilters = new PurchaseFilters();
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public ApartmentReportRepositoryNew(NadlanConext conext)
        {
            Context = conext;
        }

        //protected IEnumerable<Transaction> GetInvestment(int apartmetId)
        //{
        //    var basic = purchaseFilters.GetInvestmentFilter();
        //    return Context.Transactions
        //         .Include(a => a.Account)
        //         .Where(basic)
        //         .Where(a => a.ApartmentId == apartmetId);
        //}

        protected IEnumerable<Transaction> GetInvestment(IEnumerable<Transaction> transactions)
        {
            var basic = purchaseFilters.GetInvestmentFilter();
            return transactions
                 .Where(basic);
        }

        /// <param name="year">0 for all years</param>
        protected IEnumerable<Transaction> GetNetIncome(IEnumerable<Transaction> transactions, int year)
        {
            var basic = nonPurchaseFilters.GetProfitRemoveDistributionFilter();
            //if (year != 0) basic = t => basic(t) && t.Date.Year == year;
            var result = transactions.Where(basic);
            if (year != 0) return result.Where(a => a.Date.Year == year);
            return result;
        }

        protected IEnumerable<Transaction> GetAllNonPurchase(int apartmetId)
        {
            var basic = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
            var result = Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
            return result;
        }

        protected IEnumerable<Transaction> GetAllPurchase(int apartmetId)
        {
            var basic = purchaseFilters.GetAllPurchaseFilter();
            var result = Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
            return result;
        }

        //protected IEnumerable<Transaction> GetNetIncome(int apartmetId, int year)
        //{
        //    var basic = nonPurchaseFilters.GetProfitRemoveDistributionFilter();
        //    if (year != 0) basic = t => basic(t) && t.Date.Year == year;

        //    return Context.Transactions
        //         .Include(a => a.Account)
        //         .Where(basic)
        //         .Where(a => a.ApartmentId == apartmetId);
        //}

        //protected IEnumerable<Transaction> GetTotalCost(int apartmentId)
        //{
        //    var basic = purchaseFilters.GetTotalCostFilter();
        //    return Context.Transactions
        //          .Include(a => a.Account)
        //          .Where(basic)
        //          .Where(a => !a.Account.IsIncome)
        //          .Where(a => a.ApartmentId == apartmentId);
        //}
        protected IEnumerable<Transaction> GetTotalCost(IEnumerable<Transaction> transactions)
        {
            var basic = purchaseFilters.GetTotalCostFilter();
            return transactions.Where(basic);
        }



        public async Task<decimal> GetExpensesBalance()
        {
            var balance = Context.Expenses
                .Where(a => !a.Transaction.IsDeleted)
                .SumAsync(a => a.Transaction.Amount);
            return await balance * -1;
        }




    }

}
















