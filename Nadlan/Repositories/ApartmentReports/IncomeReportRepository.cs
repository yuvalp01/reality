using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.ApartmentReports
{
    public class IncomeReportRepository : ApartmentReportRepositoryNew
    {
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public IncomeReportRepository(NadlanConext conext) : base(conext)
        {
        }
        public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        {
            var allNonPurchase = GetAllNonPurchase(apartmentId);
            var grossIncome = GetGrossIncome(allNonPurchase, year);
            var expenses = GetAllExpenses(allNonPurchase, year);
            var netIncome = GetNetIncome(allNonPurchase, year);
            var accountSummary = GetAccountSummaryNonPurchase(allNonPurchase, year);
            IncomeReport summaryReport = new IncomeReport
            {
                GrossIncome = await Task.FromResult(grossIncome.Sum(b => b.Amount)),
                Expenses = await Task.FromResult(expenses.Sum(b => b.Amount)),
                NetIncome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
                AccountsSum = await Task.FromResult(accountSummary.ToList())

            };
            return summaryReport;
        }
        protected IEnumerable<Transaction> GetGrossIncome(IEnumerable<Transaction> transactions, int year)
        {
            var basic = nonPurchaseFilters.GetGrossIncomeFilter();
            //if (year != 0) basic = t => basic(t) && t.Date.Year == year;
            var result = transactions.Where(basic);
            if (year != 0)
            {
                return result.Where(a => a.Date.Year == year);
            }

            return result;
        }
        protected IEnumerable<Transaction> GetAllExpenses(IEnumerable<Transaction> transactions, int year)
        {
            var basic = nonPurchaseFilters.GetAllExpensesFilter();
            var result = transactions.Where(basic);
            if (year != 0) return result.Where(a => a.Date.Year == year);
            return result;
        }
        protected IEnumerable<AccountSummary> GetAccountSummaryNonPurchase(IEnumerable<Transaction> transactions, int year)
        {
            var basic = nonPurchaseFilters.GetAllExpensesFilter();
            var initResult = transactions.Where(basic);
            if (year!=0)
            {
                initResult = initResult.Where(a => a.Date.Year == year);
            }
            //if (year != 0) basic = t => basic(t) && t.Date.Year == year;

            var accountSummary = initResult
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


//protected IEnumerable<AccountSummary> GetAccountSummaryNonPurchase(int apartmetId, int year)
//{
//    var basic = nonPurchaseFilters.GetAllExpensesFilter();
//    Func<Transaction, bool> second = t => t.Date.Year == year;
//    //if (year != 0) basic = t => basic(t) && second(t);

//    var accountSummary = Context.Transactions
//        .Include(a => a.Account)
//        .Where(basic)
//        .GroupBy(g => new { g.AccountId, g.Account.Name, g.Date })
//        .OrderBy(a => a.Sum(s => s.Amount))
//        .Select(a => new AccountSummary
//        {
//            AccountId = a.Key.AccountId,
//            Name = a.Key.Name,
//            Total = a.Sum(s => s.Amount)
//        });

//    return accountSummary;
//}



//protected IEnumerable<Transaction> GetGrossIncome(int apartmetId, int year)
//{
//    var basic = nonPurchaseFilters.GetGrossIncomeFilter();
//    var result = Context.Transactions
//         .Include(a => a.Account)
//         .Where(basic)
//         .Where(a => a.ApartmentId == apartmetId);
//    if (year!=0)
//    {
//        result.Where(a => a.Date.Year == year);
//    }
//    return result;
//}





//protected IEnumerable<Transaction> GetAllExpenses(int apartmetId, int year)
//{
//    var basic = nonPurchaseFilters.GetAllExpensesFilter();
//    if (year != 0) basic = t => basic(t) && t.Date.Year == year;

//    return Context.Transactions
//         .Include(a => a.Account)
//         .Where(basic)
//         .Where(a => a.ApartmentId == apartmetId);
//}




//protected IEnumerable<Transaction> GetGrossIncome(int apartmetId, int year)
//{
//    var basic = nonPurchaseFilters.GetGrossIncomeFilter();
//    Expression<Func<Transaction, bool>> expr1 = t =>
//   basic(t) &&
//   //t.Date.Year == 2020 &&
//   t.AccountId == 1;//Rent

//    // var xxxx = basic = t => basic(t) && t.Date.Year == year;
//    Expression<Func<Transaction, bool>> expr2 = t => t.Date.Year == year;

//    var body = Expression.AndAlso(expr1.Body, expr2.Body);
//    var lambda = Expression.Lambda<Func<Transaction, bool>>(body);

//    var yyyy = Expression.And(expr1.Body, expr2.Body); 
//    var result =  Context.Transactions
//         .Include(a => a.Account)
//         .Where(lambda)
//         .Where(a => a.ApartmentId == apartmetId);
//    return result;
//}