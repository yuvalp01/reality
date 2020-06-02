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
    public class IncomeReportRepository : ApartmentReportRepositoryNew
    {
        public IncomeReportRepository(NadlanConext conext) : base(conext)
        {
        }
        public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        {
            var grossIncome = GetGrossIncome(apartmentId, year);
            var expenses = GetAllExpenses(apartmentId, year);
            var netIncome = GetNetIncome(apartmentId, year);
            var accountSummary = GetAccountSummaryNonPurchase(apartmentId, year);
            IncomeReport summaryReport = new IncomeReport
            {
                GrossIncome = await Task.FromResult(grossIncome.Sum(b => b.Amount)),
                Expenses = await Task.FromResult(expenses.Sum(b => b.Amount)),
                NetIncome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
                AccountsSum = await Task.FromResult(accountSummary.ToList())

            };
            return summaryReport;
        }
        protected IEnumerable<Transaction> GetGrossIncome(int apartmetId, int year)
        {
            var basic = NonPurchaseFilters.GetGrossIncomeFilter();
            if (year != 0) basic = t => basic(t) && t.Date.Year == year;

            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }
        protected IEnumerable<Transaction> GetAllExpenses(int apartmetId, int year)
        {
            var basic = NonPurchaseFilters.GetAllExpensesFilter();
            if (year != 0) basic = t => basic(t) && t.Date.Year == year;

            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }
        protected IEnumerable<AccountSummary> GetAccountSummaryNonPurchase(int apartmetId, int year)
        {
            var basic = NonPurchaseFilters.GetAllExpensesFilter();
            if (year != 0) basic = t => basic(t) && t.Date.Year == year;

            var accountSummary = Context.Transactions
                .Include(a => a.Account)
                .Where(basic)
                .GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = a.Sum(s => s.Amount)
                });
            return accountSummary;
        }



    }
}
