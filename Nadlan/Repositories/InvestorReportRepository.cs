using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class InvestorReportRepository : Repository<Transaction>
    {

        const decimal ANNUAL_COSTS = 100 + 350;
        public InvestorReportRepository(NadlanConext context) : base(context)
        {
        }

        public async Task<decimal> GetBalance(int accountId)
        {
            var balance = Context.Transactions.Where(a => a.AccountId == accountId).SumAsync(a => a.Amount);
            return await balance;
        }


        public async Task<InvestorReportOverview> GetInvestorReport(int investorAcountId)
        {

            var portfolioLines = Context.Portfolios.Include(a => a.Apartment).Where(a => a.AccountId == investorAcountId && a.ApartmentId != 20);
            Expression<Func<Transaction, bool>> predAll = t =>
                t.IsPurchaseCost == false
                && t.Account.AccountTypeId == 0;

            List<PortfolioReport> portfolioReportLines = new List<PortfolioReport>();
            foreach (var portfolioLine in portfolioLines)
            {
                PortfolioReport portfolioLineReport = new PortfolioReport();
                var apartment = portfolioLine.Apartment;
                portfolioLineReport.Apartment = apartment.Address;
                portfolioLineReport.PurchaseDate = apartment.PurchaseDate;
                portfolioLineReport.Ownership = portfolioLine.Percentage;
                var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();
                portfolioLineReport.Investment = totalInvestment * portfolioLine.Percentage;
                var yeardDecimal = apartment.PurchaseDate.GetApartmentYearsInDecimal();// GetApartmentYearsInDecimal(apartment.PurchaseDate);
                portfolioLineReport.MinimalProfitUpToDate = portfolioLineReport.Investment * 0.03m * yeardDecimal;
                var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == portfolioLine.ApartmentId && a.AccountId == 100);
                portfolioLineReport.Distributed = await distributed.SumAsync(a => a.Amount)*portfolioLine.Percentage * -1;
                portfolioReportLines.Add(portfolioLineReport);
            }
            var cashBalance = Context.Transactions.Where(a => a.AccountId == investorAcountId);

            var leipzigPortfolioLine = Context.Portfolios.Include(a => a.Apartment).Where(a => a.AccountId == investorAcountId && a.ApartmentId == 20).FirstOrDefault();
            if (leipzigPortfolioLine != null)
            {
                PortfolioReport portfolioLeipzigReport = new PortfolioReport();
                var apartment = leipzigPortfolioLine.Apartment;
                portfolioLeipzigReport.Apartment = apartment.Address;
                portfolioLeipzigReport.PurchaseDate = apartment.PurchaseDate;
                portfolioLeipzigReport.Ownership = leipzigPortfolioLine.Percentage;
                var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == leipzigPortfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();
                portfolioLeipzigReport.Investment = totalInvestment * leipzigPortfolioLine.Percentage;


                var netIncome = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == 20 && a.AccountId != 100);
                var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == 20 && a.AccountId == 100);
                portfolioLeipzigReport.MinimalProfitUpToDate = await netIncome.SumAsync(a => a.Amount) * leipzigPortfolioLine.Percentage;
                portfolioLeipzigReport.Distributed = await distributed.SumAsync(a => a.Amount) * leipzigPortfolioLine.Percentage *-1;
                portfolioReportLines.Add(portfolioLeipzigReport);
            }


            InvestorReportOverview investorReportOverview = new InvestorReportOverview
            {
                CashBalance = await cashBalance.SumAsync(a => a.Amount),
                TotalInvestment = portfolioReportLines.Sum(a => a.Investment),
                MinimalProfitUpToDate = portfolioReportLines.Sum(a => a.MinimalProfitUpToDate),
                PortfolioLines = portfolioReportLines,
                TotalDistribution = portfolioReportLines.Sum(a => a.Distributed),

            };
            investorReportOverview.TotalBalace = investorReportOverview.CashBalance + investorReportOverview.MinimalProfitUpToDate;
            return investorReportOverview;

        }


        private decimal GetApartmentYearsInDecimal(DateTime purchaseDate)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Today - purchaseDate;
            decimal yeardDecimal = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
            return yeardDecimal;
        }


    }
}
