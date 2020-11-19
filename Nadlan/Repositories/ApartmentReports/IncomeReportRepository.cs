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
    public class IncomeReportRepository : ApartmentReportRepository
    {
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();
        private PurchaseFilters purchaseFilters = new PurchaseFilters();

        public IncomeReportRepository(NadlanConext conext) : base(conext)
        {
        }
        public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        {

            IncomeReport incomeReport = new IncomeReport();
            incomeReport.AccountsSum = GetAccountSummaryNonPurchase(apartmentId, year);
            incomeReport.GrossIncome = GetAccountSum(apartmentId, 1, year);
            incomeReport.NetIncome = GetNetIncome(apartmentId, year);
            //summaryReport.BonusPaid = GetAccountSum(apartmentId, 300, year);
            incomeReport.Expenses = GetAllExpenses(apartmentId, year);
            decimal investment = GetAccountSum(apartmentId, 13);
            DateTime purchaseDate = Context.Apartments.Where(a => a.Id == apartmentId).First().PurchaseDate;


            if (apartmentId != 20)
            {
                if (year == 0)
                {
                    incomeReport.Bonus = -1* await Task.FromResult(CalcBonus(investment, incomeReport.NetIncome, purchaseDate, DateTime.Now));
                    incomeReport.NetForInvestor = incomeReport.NetIncome - incomeReport.Bonus;
                }
                //Do not calculate bonus so far if it's for specific year
                else
                {
                    decimal bonusPaid = GetAccountSum(apartmentId, 300, year);
                    incomeReport.NetForInvestor = incomeReport.NetIncome + bonusPaid;
                    incomeReport.Bonus = bonusPaid;
                }
            }



            return incomeReport;
        }


        protected decimal CalcBonus(decimal investment, decimal netIncome, DateTime purchaseDate, DateTime currentTime)
        {
            const double THRESHOLD = 0.03;
            const decimal PERCENTAGE = (decimal)0.5;
            decimal years = GetAgeInYears(purchaseDate);
            decimal thresholdAccumulated = CalcAccumulatedThreshold(THRESHOLD, years);
            decimal roiAccumulated = CalcAccumulatedRoi(purchaseDate, currentTime, investment, netIncome);
            //less than threshold - no bonus     
            if (roiAccumulated <= thresholdAccumulated) return 0;

            decimal bonusPercentage = (roiAccumulated - thresholdAccumulated) * PERCENTAGE;
            decimal roiForInvestor = (roiAccumulated - bonusPercentage) / years;
            return bonusPercentage * investment;
        }





        protected decimal GetAllExpenses(int apartmentId, int year)
        {
            Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.AccountId != 1 &&//Except for rent
                        t.AccountId != 100 &&//Except for distribution
                        t.AccountId != 300 &&//Except for bonus
                        t.Account.AccountTypeId == 0;

            Func<Transaction, bool> filter;
            if (year != 0)
            {
                filter = b =>
                         expensesFilter(b) &&
                         b.Date.Year == year;
            }
            else
            {
                filter = expensesFilter;
            }

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(filter)
                .Sum(a => a.Amount);
            // if (year != 0) return result.Where(a => a.Date.Year == year);
            return result;
        }




        protected List<AccountSummary> GetAccountSummaryNonPurchase(int apartmentId, int year)
        {
            //      var basic = nonPurchaseFilters.GetAllExpensesFilter();
            var basic = GetRegularTransactionsFilter(apartmentId, year, false);
            var accountSummary = Context.Transactions
                .Include(a => a.Account)
                .Where(basic)
                .Where(a => a.AccountId != 1)
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

    }
}


//public Func<Transaction, bool> GetAllExpensesFilter(int apartmentId, int year)
//{

//    //       !t.IsDeleted &&
//    //!t.IsPurchaseCost &&
//    //!t.IsBusinessExpense &&
//    //t.Account.AccountTypeId == 0;

//    Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
//    Func<Transaction, bool> expensesFilter = t =>
//                basic(t) &&
//                t.AccountId != 1 &&//Except for rent
//                t.AccountId != 100 &&//Except for distribution
//                t.AccountId != 300;//Except for bonus

//    return expensesFilter;
//}

//protected IEnumerable<Transaction> GetGrossIncome_(IEnumerable<Transaction> transactions, int year)
//{
//    var basic = nonPurchaseFilters.GetGrossIncomeFilter();
//    //if (year != 0) basic = t => basic(t) && t.Date.Year == year;
//    var result = transactions.Where(basic);
//    if (year != 0)
//    {
//        return result.Where(a => a.Date.Year == year);
//    }

//    return result;
//}
//protected IEnumerable<Transaction> GetAllExpenses_(IEnumerable<Transaction> transactions, int year)
//{
//    var basic = nonPurchaseFilters.GetAllExpensesFilter();
//    var result = transactions.Where(basic);
//    if (year != 0) return result.Where(a => a.Date.Year == year);
//    return result;
//}


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