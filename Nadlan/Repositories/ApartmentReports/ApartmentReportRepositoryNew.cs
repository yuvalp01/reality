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
        private PurchaseFilters purchaseFilters = new PurchaseFilters();
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public ApartmentReportRepository(NadlanConext conext)
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

        private decimal GetAgeInYears(DateTime date)
        {
            //double years = ((DateTime.Now - investment.date).TotalDays) / 365.255;
            //double years = DateTime.Now.Subtract(investment.date).TotalDays / 365.255;
            //System.TimeSpan diff = DateTime.Now.Subtract(investment.date) / 365.255;
            //DateTime zeroTime = new DateTime(1, 1, 1);
            //TimeSpan span = DateTime.Today - apartment.PurchaseDate;
            // decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
           // decimal years_dec = (decimal)((DateTime.Now - apartment.PurchaseDate).TotalDays) / (decimal)365.255;

            //var xxx = diff.TotalDays/365;
            decimal years_dec = (decimal)((DateTime.Now - date).TotalDays) / (decimal)365.255;
            return years_dec;

        }
        protected decimal GetBonus(decimal netIncome, int apartmentId)
        {
            //Leipzig - no bonus
            if (apartmentId == 20)
            {
                return 0;
            }
            var investmentFilter = purchaseFilters.GetInvestmentFilter();
            var investment = Context.Transactions
                .Where(investmentFilter)
                .Where(a => a.ApartmentId == apartmentId)
                .Select(a => new { amount = a.Amount, date = a.Date })
                .FirstOrDefault();
            decimal years_dec = GetAgeInYears(investment.date); //(decimal)((DateTime.Now - investment.date).TotalDays) / (decimal)365.255;

            var ROI = netIncome / investment.amount /years_dec;
            decimal bonuns = 0;
            decimal threshold = (decimal)0.03;
            if (ROI <= threshold)
            {
                return 0;
            }
            else
            {
                bonuns = (ROI - threshold) / 2 * investment.amount;
                return bonuns;
            }
        }

        protected decimal CalcROI(Apartment apartment, SummaryReport summaryReport)
        {
            if (apartment.PurchaseDate > DateTime.Now)
            {
                return 0;
            }
            decimal years_dec =GetAgeInYears(apartment.PurchaseDate);
            decimal roi = 0;
            if (summaryReport.Investment > 0 && years_dec > 0)
            {
                roi = (summaryReport.NetIncome / summaryReport.Investment) / years_dec;
            }
            return roi;
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



//protected decimal GetBonus__(SummaryReport summaryReport)
//{


//    decimal bonuns = 0;
//    decimal threshold = (decimal)0.03;
//    if (summaryReport.ROI <= threshold)
//    {
//        return 0;
//    }
//    else
//    {
//        bonuns = (summaryReport.ROI - threshold) / 2 * summaryReport.Investment;
//        return bonuns;
//    }
//}
//protected decimal GetBonus(decimal netIncome)
//{


//    decimal bonuns = 0;
//    decimal threshold = (decimal)0.03;
//    if (summaryReport.ROI <= threshold)
//    {
//        return 0;
//    }
//    else
//    {
//        bonuns = (summaryReport.ROI - threshold) / 2 * summaryReport.Investment;
//        return bonuns;
//    }
//}















