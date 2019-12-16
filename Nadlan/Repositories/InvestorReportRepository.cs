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

        //const decimal ANNUAL_COSTS = 100 + 350;
        public InvestorReportRepository(NadlanConext context) : base(context)
        {
        }

        public async Task<decimal> GetPersonalBalance(int stakeholderId)
        {
            var balance = Context.PersonalTransactions.Where(a => a.StakeholderId == stakeholderId).SumAsync(a => a.Amount);
            return await balance;
        }

        public async Task<InvestorReportOverview> GetInvestorReport(int investorAcountId)
        {
            try
            {

                // var portfolioLines = Context.Portfolios.Include(a => a.Apartment).Where(a => a.AccountId == investorAcountId && a.ApartmentId != 20);
                var portfolioLines = Context.Portfolios.Include(a => a.Apartment).Where(a => a.StakeholderId == investorAcountId && a.ApartmentId != 20);
                Expression<Func<Transaction, bool>> predAll = t =>
                    t.IsPurchaseCost == false
                    && t.Account.AccountTypeId == 0;

                List<PortfolioReport> portfolioReportLines = new List<PortfolioReport>();
                foreach (var portfolioLine in portfolioLines)
                {
                    PortfolioReport portfolioLineReport = new PortfolioReport();
                    var apartment = portfolioLine.Apartment;
                    portfolioLineReport.ApartmentId = portfolioLine.ApartmentId;
                    portfolioLineReport.Apartment = apartment.Address;
                    portfolioLineReport.PurchaseDate = apartment.PurchaseDate;
                    portfolioLineReport.Ownership = portfolioLine.Percentage;
                    //var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();


                    //var totalInvestment_ = Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount);
                    var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstOrDefaultAsync();

                    portfolioLineReport.Investment = totalInvestment * portfolioLine.Percentage;
                    var yeardDecimal = apartment.PurchaseDate.GetApartmentYearsInDecimal();
                    portfolioLineReport.MinimalProfitUpToDate = portfolioLineReport.Investment * 0.03m * yeardDecimal;
                    //var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == portfolioLine.ApartmentId && a.AccountId == 100);
                    var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == portfolioLine.ApartmentId && a.AccountId == 100);
                    portfolioLineReport.Distributed = await distributed.SumAsync(a => a.Amount) * portfolioLine.Percentage * -1;

                    ValidateWithPersonalTransactions(portfolioLineReport, portfolioLine);
                    portfolioReportLines.Add(portfolioLineReport);




                }
                // var cashBalance_old = Context.Transactions.Where(a => a.AccountId == investorAcountId);
                var cashBalance = Context.PersonalTransactions.Where(a => a.StakeholderId == investorAcountId);

                //var leipzigPortfolioLine = Context.Portfolios.Include(a => a.Apartment).Where(a => a.AccountId == investorAcountId && a.ApartmentId == 20).FirstOrDefault();
                var leipzigPortfolioLine = Context.Portfolios.Include(a => a.Apartment).Where(a => a.StakeholderId == investorAcountId && a.ApartmentId == 20).FirstOrDefault();
                if (leipzigPortfolioLine != null)
                {
                    PortfolioReport portfolioLeipzigReport = new PortfolioReport();
                    var apartment = leipzigPortfolioLine.Apartment;
                    portfolioLeipzigReport.ApartmentId = apartment.Id;
                    portfolioLeipzigReport.Apartment = apartment.Address;
                    portfolioLeipzigReport.PurchaseDate = apartment.PurchaseDate;
                    portfolioLeipzigReport.Ownership = leipzigPortfolioLine.Percentage;
                    //var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == leipzigPortfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();
                    var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == leipzigPortfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();
                    portfolioLeipzigReport.Investment = totalInvestment * leipzigPortfolioLine.Percentage;


                    var netIncome = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == 20 && a.AccountId != 100);
                    var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == 20 && a.AccountId == 100);
                    portfolioLeipzigReport.MinimalProfitUpToDate = await netIncome.SumAsync(a => a.Amount) * leipzigPortfolioLine.Percentage;
                    portfolioLeipzigReport.Distributed = await distributed.SumAsync(a => a.Amount) * leipzigPortfolioLine.Percentage * -1;
                    ValidateWithPersonalTransactions(portfolioLeipzigReport, leipzigPortfolioLine);
                    portfolioReportLines.Add(portfolioLeipzigReport);
                }


                var _cashBalance = await cashBalance.SumAsync(a => a.Amount);
                var _totalInvestment = portfolioReportLines.Sum(a => a.Investment);
                var _minimalProfitUpToDate = portfolioReportLines.Sum(a => a.MinimalProfitUpToDate);
                var _totalDistribution = portfolioReportLines.Sum(a => a.Distributed);

                string investorName = Context.Stakeholders.Find(investorAcountId).Name;
                InvestorReportOverview investorReportOverview = new InvestorReportOverview
                {
                    Name = investorName,
                    CashBalance = _cashBalance,
                    TotalInvestment = _totalInvestment,
                    MinimalProfitUpToDate = _minimalProfitUpToDate,
                    PortfolioLines = portfolioReportLines,
                    TotalDistribution = _totalDistribution,

                };
                investorReportOverview.TotalBalace = investorReportOverview.CashBalance + investorReportOverview.MinimalProfitUpToDate;
                return investorReportOverview;

            }
            catch (Exception)
            {
                return new InvestorReportOverview { CashBalance = 0, MinimalProfitUpToDate = 0, PortfolioLines = new List<PortfolioReport>(), TotalBalace = 0, TotalDistribution = 0, TotalInvestment = 0 };
            }


        }


        private void ValidateWithPersonalTransactions(PortfolioReport portfolioLineReport, Portfolio portfolioLine)
        {
            if (portfolioLine.Percentage < 1)
            {
                var personalInvestment = Context.PersonalTransactions
                    .Where(a =>
                    a.ApartmentId == portfolioLine.ApartmentId &&
                    a.StakeholderId == portfolioLine.StakeholderId &&
                    a.TransactionType == TransactionType.Commitment).Sum(a => a.Amount);
                if (portfolioLineReport.Investment != Math.Abs(personalInvestment))
                {
                    throw new Exception("Personal investment does not equal to general investment.");
                    //portfolioLineReport.Investment = 0;

                }
                var personalDistribution = Context.PersonalTransactions
                    .Where(a =>
                    a.ApartmentId == portfolioLine.ApartmentId &&
                    a.StakeholderId == portfolioLine.StakeholderId &&
                    (a.TransactionType == TransactionType.Distribution|| a.TransactionType== TransactionType.ReminderDistribution)).Sum(a => a.Amount);
                if (Math.Round(portfolioLineReport.Distributed) != Math.Round(personalDistribution))
                {
                    //portfolioLineReport.Distributed = 0;
                    throw new Exception("Personal distribution does not equal to general distribution.");

                }
            }

        }


        private List<PortfolioReport> CreatePortfolioReports(IQueryable<Portfolio> portfolioLines)
        {
            Expression<Func<Transaction, bool>> predAll = t =>
                                                t.IsPurchaseCost == false
                                                && t.Account.AccountTypeId == 0;
            List<PortfolioReport> portfolioReportLines = new List<PortfolioReport>();
            foreach (var portfolioLine in portfolioLines)
            {
                PortfolioReport portfolioLineReport = new PortfolioReport();
                var apartment = portfolioLine.Apartment;
                portfolioLineReport.ApartmentId = portfolioLine.ApartmentId;
                portfolioLineReport.Apartment = apartment.Address;
                portfolioLineReport.PurchaseDate = apartment.PurchaseDate;
                portfolioLineReport.Ownership = portfolioLine.Percentage;
                //var totalInvestment = await Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstAsync();
                var totalInvestment = Context.Transactions.Where(a => a.ApartmentId == portfolioLine.Apartment.Id && a.AccountId == 13).Select(a => a.Amount).FirstOrDefault();
                portfolioLineReport.Investment = totalInvestment * portfolioLine.Percentage;
                var yeardDecimal = apartment.PurchaseDate.GetApartmentYearsInDecimal();
                portfolioLineReport.MinimalProfitUpToDate = portfolioLineReport.Investment * 0.03m * yeardDecimal;
                //var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == portfolioLine.ApartmentId && a.AccountId == 100);
                var distributed = Context.Transactions.Include(a => a.Account).Where(predAll).Where(a => a.ApartmentId == portfolioLine.ApartmentId && a.AccountId == 100);
                portfolioLineReport.Distributed = distributed.Sum(a => a.Amount) * portfolioLine.Percentage * -1;
                portfolioReportLines.Add(portfolioLineReport);
            }
            return portfolioReportLines;
        }


    }
}
