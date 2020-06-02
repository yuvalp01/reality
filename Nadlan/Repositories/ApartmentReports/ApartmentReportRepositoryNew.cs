using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class ApartmentReportRepositoryNew
    {

        protected NadlanConext Context { get; set; }
        public ApartmentReportRepositoryNew(NadlanConext conext)
        {
            Context = conext;
        }

        protected IEnumerable<Transaction> GetInvestment(int apartmetId)
        {
            var basic = PurchaseFilters.GetInvestmentFilter();
            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a=>a.ApartmentId ==apartmetId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apartmetId"></param>
        /// <param name="year">0 for all years</param>
        /// <returns></returns>
        protected IEnumerable<Transaction> GetNetIncome(int apartmetId, int year)
        {
            var basic = NonPurchaseFilters.GetProfitRemoveDistributionFilter();
            if (year != 0) basic = t => basic(t) && t.Date.Year == year;

            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }

        protected IEnumerable<Transaction> GetTotalCost(int apartmentId)
        {
            var basic = PurchaseFilters.GetTotalCostFilter();
            return Context.Transactions
                  .Include(a => a.Account)
                  .Where(basic)
                  .Where(a => !a.Account.IsIncome)
                  .Where(a => a.ApartmentId==apartmentId);
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
















