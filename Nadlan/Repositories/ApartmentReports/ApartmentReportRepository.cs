﻿using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using System;
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


        //protected Func<Transaction, bool> GetAllValidTransactionsForReports(List<int> apartmetIds)
        //{
        //    Func<Transaction, bool> result = t =>
        //         t.IsDeleted == false &&
        //         t.IsBusinessExpense == false &&
        //         apartmetIds.Contains(t.ApartmentId);
        //    return result;
        //}
        protected Func<Transaction, bool> GetAllValidTransactionsForReports(int apartmetId)
        {
            Func<Transaction, bool> result = t =>
                 t.IsDeleted == false &&
                 t.IsBusinessExpense == false &&
                 t.ApartmentId == apartmetId;
            return result;
        }

        protected decimal GetAccountSum(int apartmentId, int accountId, int year, bool isSoFar = false)
        {
            var predicate = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> filter;
            if (year != 0)
            {
                if (isSoFar)
                {
                    filter = b =>
                         predicate(b) &&
                         b.Date.Year <= year;
                }
                else
                {
                    filter = b =>
                             predicate(b) &&
                             b.Date.Year == year;
                }

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

        public decimal GetAccountSum(int apartmentId, int accountId)
        {
            var predicate = GetAllValidTransactionsForReports(apartmentId);
            return Context.Transactions
                 .Where(predicate)
                 .Where(a => a.AccountId == accountId)
                 .Sum(a => a.Amount);
        }
        public decimal GetAccountSum(int apartmentId, int accountId, DateTime currentDate)
        {
            var predicate = GetAllValidTransactionsForReports(apartmentId);
            return Context.Transactions
                 .Where(predicate)
                 .Where(a => a.AccountId == accountId)
                 .Where(a=>a.Date<=currentDate)
                 .Sum(a => a.Amount);
        }

        /// <param name="year">0 for all years</param>
        public decimal GetNetIncomeNew(int apartmentId, DateTime currentDate, bool isAggregated)
        {
            var predicate = GetRegularTransactionsFilterNew(apartmentId,false);
            Func<Transaction, bool> filter;
            if (isAggregated)
            {
                filter = b =>
                predicate(b) &&
                b.Date <= currentDate;
            }
            else
            {
                filter = b =>
                predicate(b) &&
                b.Date == currentDate;
            }
            var result = Context.Transactions
              //  .Include(a => a.Account)
                .Where(filter).Sum(a => a.Amount);
            return result;
        }


        /// <param name="year">0 for all years</param>
        public decimal GetNetIncome(int apartmentId, int year)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId, year, false);

            var result = Context.Transactions
                //.Include(a => a.Account)
                .Where(predicate).Sum(a => a.Amount);
            return result;
        }

        public decimal GetBonus(int apartmentId,DateTime purchaseDate,DateTime currentDate, out decimal investment, out decimal netIncome)
        {
            netIncome = GetNetIncome(apartmentId, 0);
            investment = GetAccountSum(apartmentId, 13);

            if (apartmentId==20)
            {
                return 0;
            }
            var bonus = CalcBonus(investment, netIncome, purchaseDate, currentDate);

            return bonus;
        }

        /// <param name="year">0 for all years</param>
        protected decimal GetTotalCost(int apartmentId)
        {
            var predicate = GetRegularTransactionsFilter(apartmentId, 0, true);

            var result = Context.Transactions
                .Include(a => a.Account)
                .Where(predicate)
                .Where(a => a.Account.IsIncome == false)
                .Where(a => a.AccountId != 13)  //not investment
                .Sum(a => a.Amount);
            return result;
        }

        //protected IEnumerable<Transaction> GetTotalCost(IEnumerable<Transaction> transactions)
        //{
        //    var basic = purchaseFilters.GetTotalCostFilter();
        //    return transactions.Where(basic);
        //}





        protected Func<Transaction, bool> GetRegularTransactionsFilterNew(int apartmentId, bool isPurchaseCost)
        {
            Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> filter = t =>
                        basic(t) &&
                        t.IsPurchaseCost == isPurchaseCost &&
                        t.AccountId != 100 &&//Except for distribution
                        t.AccountId != 198 &&//Except for deposit
                        t.AccountId != 200 &&//Except for business
                        t.AccountId != 201 &&//Except for balance
                        t.AccountId != 300;//Except for bonus
                       // t.Account.AccountTypeId == 0;

            return filter;
        }





        protected Func<Transaction, bool> GetRegularTransactionsFilter(int apartmentId, int year, bool isPurchaseCost)
        {
            Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.IsPurchaseCost == isPurchaseCost &&
                        t.AccountId != 198 &&//Except for deposit
                        t.AccountId != 200 &&//Except for business
                        t.AccountId != 201 &&//Except for balance
                        t.AccountId != 100 &&//Except for distribution
                        t.AccountId != 300;//Except for bonus
                        //t.Account.AccountTypeId == 0;

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

        public decimal CalcBonus(decimal investment, decimal netIncome, DateTime purchaseDate, DateTime currentDate)
        {
            const double THRESHOLD = 0.03;
            const decimal PERCENTAGE = (decimal)0.5;
            decimal years = GetAgeInYears(purchaseDate, currentDate);
            decimal thresholdAccumulated = CalcAccumulatedThreshold(THRESHOLD, years);
            decimal roiAccumulated = CalcAccumulatedRoi(purchaseDate, currentDate, investment, netIncome);
            //less than threshold - no bonus     
            if (roiAccumulated <= thresholdAccumulated) return 0;

            decimal bonusPercentage = (roiAccumulated - thresholdAccumulated) * PERCENTAGE;
            decimal roiForInvestor = (roiAccumulated - bonusPercentage) / years;
            return bonusPercentage * investment;
        }



        protected decimal GetAgeInYears(DateTime date, DateTime currentDate)
        {
            return (decimal)((currentDate - date).TotalDays) / (decimal)365.255;

        }

        protected decimal CalcAccumulatedRoi(DateTime purchaseDate, DateTime currectTime, decimal investment, decimal netIncome)// SummaryReport summaryReport)
        {
            if (purchaseDate >= currectTime || investment <= 0)
            {
                return 0;
            }
            return (netIncome / investment);

        }




        //protected IEnumerable<Transaction> GetAllTransactions(int apartmetId)
        //{
        //    var result = Context.Transactions
        //         .Include(a => a.Account)
        //         .Where(a => a.IsDeleted == false)
        //         .Where(a => a.ApartmentId == apartmetId);
        //    return result;
        //}

        //protected IEnumerable<Transaction> GetAllNonPurchase(int apartmetId)
        //{
        //    var basic = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
        //    var result = Context.Transactions
        //         .Include(a => a.Account)
        //         .Where(basic)
        //         .Where(a => a.ApartmentId == apartmetId);
        //    return result;
        //}

        //protected IEnumerable<Transaction> GetAllPurchase(int apartmetId)
        //{
        //    var basic = purchaseFilters.GetAllPurchaseFilter();
        //    var result = Context.Transactions
        //         .Include(a => a.Account)
        //         .Where(basic)
        //         .Where(a => a.ApartmentId == apartmetId);
        //    return result;
        //}


        public async Task<decimal> GetExpensesBalance()
        {
            var balance = Context.Expenses
                .Where(a => !a.Transaction.IsDeleted)
                .SumAsync(a => a.Transaction.Amount);
            return await balance * -1;
        }

    }

}







