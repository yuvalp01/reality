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
    public class ApartmentReportRepository
    {

        protected NadlanConext Context { get; set; }
        protected TransactionRepository transactionRepository { get; set; }
        private PurchaseFilters purchaseFilters = new PurchaseFilters();
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public ApartmentReportRepository(NadlanConext conext)
        {
            Context = conext;
            transactionRepository = new TransactionRepository(conext);
        }


        protected Func<Transaction, bool> GetAllValidTransactionsForReports(int apartmetId)
        {
            Func<Transaction, bool> result = t =>
                 t.IsDeleted == false &&
                 t.IsBusinessExpense == false &&
                 t.ApartmentId == apartmetId;
            return result;
        }

        protected decimal GetAccountSum(int apartmentId, int accountId, int year)
        {
            var predicate = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> filter;
            if (year != 0)
            {
                filter = b =>
                         predicate(b) &&
                         b.Date.Year == year;
            }
            else
            {
                filter = predicate;
            }
            return Context.Transactions
                 .Where(filter)
                 .Where(a => a.AccountId == accountId)
                 .Sum(a => a.Amount);
        }

        protected decimal GetAccountSum(int apartmentId, int accountId)
        {
            var predicate = GetAllValidTransactionsForReports(apartmentId);
            return Context.Transactions
                 .Where(predicate)
                 .Where(a => a.AccountId == accountId)
                 .Sum(a => a.Amount);
        }

        /// <param name="year">0 for all years</param>
        protected decimal GetNetIncome(int apartmentId, int year)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId,year, false);

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(predicate).Sum(a=>a.Amount);
            return result;
        }

        /// <param name="year">0 for all years</param>
        protected decimal GetTotalCost(int apartmentId)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId, 0, true);

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(predicate)
                .Where(a=>a.Account.IsIncome==false)
                .Where(a=>a.AccountId!=13)  //not investment
                .Sum(a => a.Amount);
            return result;
        }

        protected IEnumerable<Transaction> GetTotalCost(IEnumerable<Transaction> transactions)
        {
            var basic = purchaseFilters.GetTotalCostFilter();
            return transactions.Where(basic);
        }


        /// <param name="year">0 for all years</param>
        protected decimal GetMaintenanceExpenses(int apartmentId, int year)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId, year, false);

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(predicate)
                //Remove rent
                .Where(a=>a.AccountId!=1)
                .Sum(a => a.Amount);
            return result;
        }

        /// <param name="year">0 for all years</param>
        protected decimal GetPurchaseExpenses(int apartmentId, int year)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId, year, true);

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(predicate)
                .Sum(a => a.Amount);
            return result;
        }




        protected Func<Transaction, bool> GetRegularTransactionsFilter(int apartmentId, int year, bool isPurchaseCost)
        {
            Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.IsPurchaseCost==isPurchaseCost &&
                        //t.AccountId != 1 &&//Except for rent
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
            return filter;
        }


        //var result = Context.Transactions
        //    .Include(a => a.Account)
        //    .Where(filter)
        //    .Sum(a => a.Amount);
        //// if (year != 0) return result.Where(a => a.Date.Year == year);
        //return result;
        //}





        ///// <param name="year">0 for all years</param>
        //protected decimal GetNetIncome(int apartmentId)
        //{
        //    var basic = GetAllValidTransactionsForReports(apartmentId);
        //    //var basic = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
        //    //if (year != 0) basic = t => basic(t) && t.Date.Year == year;
        //    var result = Context.Transactions
        //        .Include(a => a.Account)
        //        .Where(basic)
        //        .Where(a => a.IsPurchaseCost == false)//remove distribution
        //        .Where(a => a.AccountId != 100)//remove distribution
        //        .Where(a => a.AccountId != 300)//remove bonus
        //        .Where(a => a.Account.AccountTypeId == 0);
        //    //if (year != 0) return result.Where(a => a.Date.Year == year);
        //    return result;
        //}


        ///// <param name="year">0 for all years</param>
        //protected IEnumerable<Transaction> GetNetIncome(int apartmentId, int year)
        //{
        //    var basic = GetAllValidTransactionsForReports(apartmentId);
        //    //var basic = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
        //    //if (year != 0) basic = t => basic(t) && t.Date.Year == year;
        //    var result = Context.Transactions
        //        .Include(a => a.Account)
        //        .Where(basic)
        //        .Where(a => a.IsPurchaseCost == false)//remove distribution
        //        .Where(a => a.AccountId != 100)//remove distribution
        //        .Where(a => a.AccountId != 300)//remove bonus
        //        .Where(a => a.Account.AccountTypeId == 0);
        //    if (year != 0) return result.Where(a => a.Date.Year == year);
        //    return result;
        //}

        //[Obsolete]
        ///// <param name="year">0 for all years</param>
        //protected IEnumerable<Transaction> GetBonusPaid(IEnumerable<Transaction> transactions, int year)
        //{
        //    var result = transactions
        //        .Where(a => a.AccountId == 300);//remove bonus
        //    if (year != 0) return result.Where(a => a.Date.Year == year);
        //    return result;
        //}

        protected decimal CalcAccumulatedThreshold(double threshold, decimal years)
        {
            if (years > 1)
            {
                return (decimal)Math.Pow((1 + threshold), (double)years) - 1;
            }
            else
            {
                return (decimal)threshold * years;
            }
        }

        protected IEnumerable<Transaction> GetBonusPaid(int apartmentId, int year)
        {
            var basic = GetAllValidTransactionsForReports(apartmentId);
            var result = Context.Transactions
                .Where(basic)
                .Where(a => a.AccountId == 300);
            if (year != 0) return result.Where(a => a.Date.Year == year);
            return result;
        }




        protected decimal GetAgeInYears(DateTime date)
        {
            return (decimal)((DateTime.Now - date).TotalDays) / (decimal)365.255;

        }
        //protected decimal GetCalculatedBonus(decimal netIncome, int apartmentId)
        //{
        //    //Leipzig - no bonus
        //    if (apartmentId == 20)
        //    {
        //        return 0;
        //    }
        //    var investmentFilter = purchaseFilters.GetInvestmentFilter();
        //    var investment = Context.Transactions
        //        .Where(investmentFilter)
        //        .Where(a => a.ApartmentId == apartmentId)
        //        .Select(a => new { amount = a.Amount, date = a.Date })
        //        .FirstOrDefault();
        //    decimal years_dec = GetAgeInYears(investment.date); //(decimal)((DateTime.Now - investment.date).TotalDays) / (decimal)365.255;

        //    var ROI = netIncome / investment.amount / years_dec;
        //    decimal bonuns = 0;
        //    decimal threshold = (decimal)0.03;
        //    if (ROI <= threshold)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        decimal bonusPercentage = (ROI - threshold) / 2;
        //        bonuns = bonusPercentage * years_dec * investment.amount;
        //        return bonuns;
        //    }
        //}


        protected decimal CalcAccumulatedRoi(DateTime purchaseDate, DateTime currectTime, decimal investment, decimal netIncome)// SummaryReport summaryReport)
        {
            if (purchaseDate >= currectTime || investment <= 0)
            {
                return 0;
            }
            return (netIncome / investment);

        }


        protected decimal CalcROI(Apartment apartment, SummaryReport summaryReport)
        {
            if (apartment.PurchaseDate > DateTime.Now)
            {
                return 0;
            }
            decimal years_dec = GetAgeInYears(apartment.PurchaseDate);
            decimal roi = 0;
            if (summaryReport.Investment > 0 && years_dec > 0)
            {
                roi = (summaryReport.NetIncome / summaryReport.Investment) / years_dec;
            }
            return roi;
        }


        protected IEnumerable<Transaction> GetAllTransactions(int apartmetId)
        {
            var result = Context.Transactions
                 .Include(a => a.Account)
                 .Where(a => a.IsDeleted == false)
                 .Where(a => a.ApartmentId == apartmetId);
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



        protected IEnumerable<Transaction> GetTotalCost_(IEnumerable<Transaction> transactions)
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









//protected IEnumerable<Transaction> GetNetIncome(int apartmetId, int year)
//{
//    var basic = nonPurchaseFilters.GetProfitRemoveDistributionFilter();
//    if (year != 0) basic = t => basic(t) && t.Date.Year == year;

//    return Context.Transactions
//         .Include(a => a.Account)
//         .Where(basic)
//         .Where(a => a.ApartmentId == apartmetId);
//}

//protected decimal GetInvestment(int apartmentId)
//{
//    var predicate = GetAllValidTransactionsForReports(apartmentId);
//    return Context.Transactions
//         .Where(predicate)
//         .Where(a=>a.AccountId==13)
//         .Sum(a=>a.Amount);
//}


//protected IEnumerable<Transaction> GetInvestment(IEnumerable<Transaction> transactions)
//{
//    var basic = purchaseFilters.GetInvestmentFilter();
//    return transactions
//         .Where(basic);
//}

//public Func<Transaction, bool> GetProfitIncludingDistributionsFilter()
//{
//    Func<Transaction, bool> basicPredicate = t =>
//                !t.IsDeleted &&
//                !t.IsPurchaseCost &&
//                !t.IsBusinessExpense &&
//                t.Account.AccountTypeId == 0;
//    //t.AccountId != 198; //Not a deposit (this line is a not needed as we have the AccountTypeId condition)
//    return basicPredicate;
//}

//double years = ((DateTime.Now - investment.date).TotalDays) / 365.255;
//double years = DateTime.Now.Subtract(investment.date).TotalDays / 365.255;
//System.TimeSpan diff = DateTime.Now.Subtract(investment.date) / 365.255;
//DateTime zeroTime = new DateTime(1, 1, 1);
//TimeSpan span = DateTime.Today - apartment.PurchaseDate;
// decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
// decimal years_dec = (decimal)((DateTime.Now - apartment.PurchaseDate).TotalDays) / (decimal)365.255;

//var xxx = diff.TotalDays/365;


//protected IEnumerable<Transaction> GetInvestment(int apartmetId)
//{
//    var basic = purchaseFilters.GetInvestmentFilter();
//    return Context.Transactions
//         .Include(a => a.Account)
//         .Where(basic)
//         .Where(a => a.ApartmentId == apartmetId);
//}
