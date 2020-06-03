using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class InvestorReportRepository : Repository<Transaction>
    {
        private PurchaseFilters purchaseFilters = new PurchaseFilters();
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public InvestorReportRepository(NadlanConext context) : base(context)
        {
        }

        public async Task<InvestorReportOverview> GetInvestorReport(int investorId)
        {
            try
            {
                IQueryable<Portfolio> portfolioLines = GetPortfolioLines(investorId);
                List<PortfolioReport> portfolioReportLines = await GetPortfolioReportLines(portfolioLines);
                var totalPendingProfits = portfolioReportLines.Sum(a => a.PendingProfits);
                var totalDistribution = portfolioReportLines.Sum(a => a.Distributed);
                InvestorReportOverview investorReportOverview = new InvestorReportOverview
                {
                    Name = Context.Stakeholders.Find(investorId).Name,
                    CashBalance = await GetPersonalBalance(investorId),
                    TotalInvestment = portfolioReportLines.Sum(a => a.Investment),
                    PortfolioLines = portfolioReportLines,
                    TotalPendingProfits = totalPendingProfits,
                    TotalDistribution = totalDistribution,
                    //ProfitsSoFar = totalPendingProfits + totalDistribution
                };
                investorReportOverview.TotalBalace = investorReportOverview.CashBalance
                    + investorReportOverview.TotalPendingProfits;
                return investorReportOverview;
            }
            catch (Exception)
            {
                return new InvestorReportOverview
                {
                    CashBalance = 0,
                    PortfolioLines = new List<PortfolioReport>(),
                    TotalBalace = 0,
                    TotalInvestment = 0
                };
            }
        }


        private IQueryable<Portfolio> GetPortfolioLines(int investorId)
        {
            var portfolioLines = Context.Portfolios
                                    .Include(a => a.Apartment)
                                    .Where(a => a.StakeholderId == investorId);
            return portfolioLines;
        }

        private async Task<List<PortfolioReport>> GetPortfolioReportLines(IQueryable<Portfolio> portfolioLines)
        {
            List<PortfolioReport> portfolioReportLines = new List<PortfolioReport>();
            foreach (var portfolioLine in portfolioLines)
            {
                PortfolioReport portfolioLineReport = new PortfolioReport();
                var apartment = portfolioLine.Apartment;
                portfolioLineReport.ApartmentId = portfolioLine.ApartmentId;
                portfolioLineReport.Apartment = apartment.Address;
                portfolioLineReport.PurchaseDate = apartment.PurchaseDate;
                portfolioLineReport.Ownership = portfolioLine.Percentage;
                portfolioLineReport.Investment =  GetTotalInvestment(portfolioLine) * portfolioLine.Percentage;
                portfolioLineReport.PendingProfits = GetPendingProfit(portfolioLine) * portfolioLine.Percentage;
                portfolioLineReport.Distributed =  GetGeneralDistributionPerInvestor(portfolioLine) * -1 * portfolioLine.Percentage;
                ValidateWithPersonalTransactions(portfolioLineReport, portfolioLine);
                portfolioReportLines.Add(portfolioLineReport);
            }
            return portfolioReportLines;
        }

        private decimal GetTotalInvestment(Portfolio portfolioLine)
        {
            var distributionPredicate = purchaseFilters.GetInvestmentFilter();

            var totalInvestment = Context.Transactions
                .Where(distributionPredicate)
                .Where(a => a.ApartmentId == portfolioLine.Apartment.Id)                        
                .Select(a => a.Amount).FirstOrDefault();
            return totalInvestment;
        }
        private decimal GetGeneralDistributionPerInvestor(Portfolio portfolioLine)
        {
            var distributionPredicate = nonPurchaseFilters.GetAllDistributionsFilter();

            var distributed = Context.Transactions
                .Include(a => a.Account)
                .Where(distributionPredicate)
                .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
                .AsQueryable();
            return distributed.Sum(a => a.Amount);
        }


        private decimal GetPendingProfit(Portfolio portfolioLine)
        {
            var profitPredicate = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
            var profit = Context.Transactions
                .Include(a => a.Account)
                .Where(profitPredicate)
                .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
                .AsQueryable()
                .Sum(a => a.Amount);
            //var profitToInvestor = profit;
            return profit;
        }
        //private decimal GetPendingProfitPerInvestor_(Portfolio portfolioLine)
        //{
        //    var profitPredicate = PredicateFilters.GetProfitAfterDistributionFilter();
        //    var profit = Context.Transactions
        //        .Include(a => a.Account)
        //        .Where(profitPredicate)
        //        .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
        //        .AsQueryable()
        //        .SumAsync(a => a.Amount);
        //    var profitToInvestor = profit.Result * portfolioLine.Percentage * -1;
        //    return profitToInvestor;
        //}

        public async Task<decimal> GetPersonalBalance(int stakeholderId)
        {
            var balance = Context.PersonalTransactions
                .Where(a => !a.IsDeleted)
                .Where(a => a.StakeholderId == stakeholderId)
                .SumAsync(a => a.Amount);
            return await balance;
        }

        private void ValidateWithPersonalTransactions(PortfolioReport portfolioLineReport, Portfolio portfolioLine)
        {
            if (portfolioLine.Percentage < 1)
            {
                var personalInvestment = Context.PersonalTransactions
                    .Where(a => !a.IsDeleted)
                    .Where(a =>
                    a.ApartmentId == portfolioLine.ApartmentId &&
                    a.StakeholderId == portfolioLine.StakeholderId &&
                    a.TransactionType == TransactionType.PaidOnBefalf).Sum(a => a.Amount);
                if (Math.Round(portfolioLineReport.Investment) != Math.Round(Math.Abs(personalInvestment)))
                {
                    throw new Exception("Personal investment does not equal to general investment.");

                }
                var personalDistribution = Context.PersonalTransactions
                    .Where(a => !a.IsDeleted)
                    .Where(a =>
                    a.ApartmentId == portfolioLine.ApartmentId &&
                    a.StakeholderId == portfolioLine.StakeholderId &&
                    //(a.TransactionType == TransactionType.Distribution || a.TransactionType == TransactionType.ReminderDistribution)).Sum(a => a.Amount);
                    a.TransactionType == TransactionType.Distribution).Sum(a => a.Amount);
                if (Math.Round(portfolioLineReport.Distributed) != Math.Round(personalDistribution))
                {
                    throw new Exception("Personal distribution does not equal to general distribution.");

                }
            }

        }


    }
}
//private Task<decimal> GetCashBalance(int investorId)
//{
//    var cashBalance = Context.PersonalTransactions
//                        .Where(a => !a.IsDeleted)
//                        .Where(a => a.StakeholderId == investorId)
//                        .SumAsync(a => a.Amount);
//    return cashBalance;
//}



//private decimal GetPendingProfitPerInvestor_(Portfolio portfolioLine)
//{
//    int[] ExcludedAccounts = { 100, 198 };//Distribution and deposit account account.
//    var distributed = Context.Transactions
//        .Include(a => a.Account)
//        .Where(a => !a.IsDeleted)
//        .Where(a => !a.IsPurchaseCost)
//        .Where(a => !a.IsBusinessExpense)
//        .Where(a => a.Account.AccountTypeId == 0)
//        .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
//        .Where(a => ExcludedAccounts.Contains(a.AccountId));

//    return distributed.SumAsync(a => a.Amount).Result * portfolioLine.Percentage * -1;
//}

//private decimal GetPendingProfit(int investorId)
//{
//    //TODO
//    //var xxxxx = CalculateGeneralPendingDistributionPerInvestor()
//    return 0;


//private decimal GetGeneralDistributionPerInvestor(Portfolio portfolioLine)
//{
//    var distributionPredicate = PredicateFilters.GetBasicDistributionFilter();

//    var distributed = Context.Transactions
//        .Include(a => a.Account)
//        .Where(distributionPredicate)
//        .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
//        .AsQueryable();
//    return distributed.SumAsync(a => a.Amount).Result * portfolioLine.Percentage * -1;
//}