﻿using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.Repositories.ApartmentReports;
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
        private ApartmentReportRepository _apartmentReportRepository;
        public InvestorReportRepository(NadlanConext context) : base(context)
        {
            _apartmentReportRepository = new ApartmentReportRepository(context);
        }

        public async Task<InvestorReportOverview> GetInvestorReport(int investorId, DateTime currentDate)
        {
            try
            {
                IQueryable<Portfolio> portfolioLines = GetPortfolioLines(investorId);
                List<PortfolioReport> portfolioReportLines = GetPortfolioReportLines(portfolioLines, currentDate);
                InvestorReportOverview investorReportOverview = new InvestorReportOverview();
                investorReportOverview.Name = Context.Stakeholders.Find(investorId).Name;
                investorReportOverview.CashBalance = await GetPersonalBalance(investorId);

                investorReportOverview.TotalInvestment = portfolioReportLines.Sum(a => a.Investment);
                investorReportOverview.TotalPendingProfits = portfolioReportLines.Sum(a => a.PendingProfits);
                investorReportOverview.TotalPendingBonus = portfolioReportLines.Sum(a => a.PendingBonus);
                //investorReportOverview.TotalDistribution = portfolioReportLines.Sum(a => a.Distributed);
                investorReportOverview.TotalNetProfit = portfolioReportLines.Sum(a => a.NetProfit);
                investorReportOverview.TotalPendingExpenses = portfolioReportLines.Sum(a => a.PendingExpenses);
                investorReportOverview.TotalBalace = 
                    investorReportOverview.CashBalance
                    + investorReportOverview.TotalPendingProfits
                    + investorReportOverview.TotalPendingExpenses;
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


        public List<PortfolioReport> GetPortfolio(int investorId, DateTime currentDate)
        {
            IQueryable<Portfolio> portfolioLines = GetPortfolioLines(investorId);
            List<PortfolioReport> portfolioReportLines = GetPortfolioReportLines(portfolioLines, currentDate);
            return portfolioReportLines;
        }


        public IQueryable<Portfolio> GetPortfolioLines(int investorId)
        {
            var portfolioLines = Context.Portfolios
                                    .Include(a => a.Apartment)
                                    .Where(a => a.StakeholderId == investorId)
                                    .Where(a=>a.Apartment.Status>=0);
            return portfolioLines;
        }



        private List<PortfolioReport> GetPortfolioReportLines(IQueryable<Portfolio> portfolioLines, DateTime currentDate)
        {
            //TODO: move this logic to the server side
            List<int> partnershipApartments = new List<int> { 1, 3, 4, 20 };
            List<PortfolioReport> portfolioReportLines = new List<PortfolioReport>();
            foreach (var portfolioLine in portfolioLines)
            {
                PortfolioReport portfolioLineReport = new PortfolioReport();
                var apartment = portfolioLine.Apartment;
                portfolioLineReport.ApartmentId = portfolioLine.ApartmentId;
                portfolioLineReport.Apartment = apartment.Address;
                portfolioLineReport.PurchaseDate = apartment.PurchaseDate;
                portfolioLineReport.Ownership = portfolioLine.Percentage;
                decimal investment;
                decimal netIncome;

                var bonusSoFar = _apartmentReportRepository
                    .GetBonus(portfolioLine.ApartmentId,
                    portfolioLine.Apartment.PurchaseDate,
                    currentDate,
                    out investment,
                    out netIncome);
                portfolioLineReport.Investment = investment * portfolioLine.Percentage; //GetTotalInvestment(portfolioLine) * portfolioLine.Percentage;
                portfolioLineReport.NetProfit = (netIncome - bonusSoFar) * portfolioLine.Percentage;
                if (apartment.OwnershipType == (int) OwnershipType.Investor)
                {
                    portfolioLineReport.NetProfit = netIncome;
                }
           
                //In a partenership apartments all expenses included in the apartment funds
                if (partnershipApartments.Contains(portfolioLine.ApartmentId))
                {
                    portfolioLineReport.Distributed = GetGeneralDistributionPerInvestor(portfolioLine) * -1 * portfolioLine.Percentage;
                    //portfolioLineReport.PendingProfits = GetPendingProfit(portfolioLine) * portfolioLine.Percentage;
                    portfolioLineReport.PendingProfits = portfolioLineReport.NetProfit - portfolioLineReport.Distributed;
                    portfolioLineReport.PendingExpenses = 0;
                }
                //In full ownership apartments there is no need in distribution because it's 100% ownership
                //and it's already in their bank account
                //On the other hand, they need to pay expenses and pending bonus
                else
                {
                    portfolioLineReport.PendingProfits = 0;
                    portfolioLineReport.Distributed = netIncome * portfolioLine.Percentage;
                    portfolioLineReport.PendingExpenses = GetPendingExpenses(portfolioLine.ApartmentId) * portfolioLine.Percentage;
                    var bonusPaid = _apartmentReportRepository.GetAccountSum(portfolioLine.ApartmentId, 300);
                    portfolioLineReport.PendingBonus = -1 * (bonusSoFar + bonusPaid) * portfolioLine.Percentage;

                    //var bonusPaid = _apartmentReportRepository.GetAccountSum(portfolioLine.ApartmentId, 300);
                    //portfolioLineReport.PendingBonus = GetPendingBonus(portfolioLine) - bonusPaid;
                }

                ValidateWithPersonalTransactions(portfolioLineReport, portfolioLine);
                portfolioReportLines.Add(portfolioLineReport);
            }
            return portfolioReportLines;
        }

        public decimal GetPendingExpenses(int apartmentId)
        {

           // Func<Transaction, bool> expensesFilter = t =>
           //!t.IsDeleted &&
           // t.PersonalTransactionId == 0;//Not covered yet
            var expenses = Context.Transactions
                .Where(a => a.IsDeleted == false)
                .Where(a => a.IsBusinessExpense == false)
                .Where(a => a.PersonalTransactionId == 0) //Not covered yet
                .Where(a => a.ApartmentId == apartmentId)
                .Sum(a => a.Amount);
            return expenses;
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

            var distributed = Context.Transactions
                .Include(a => a.Account)
                .Where(a => a.IsDeleted == false)
                .Where(a => a.AccountId == 100)
                .Where(a => a.ApartmentId == portfolioLine.ApartmentId);
            return distributed.Sum(a => a.Amount);
        }





        private decimal GetPendingProfit(Portfolio portfolioLine)
        {
            var profitPredicate = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();
            var profit = Context.Transactions
                .Include(a => a.Account)
                .Where(profitPredicate)
                .Where(a => a.ApartmentId == portfolioLine.ApartmentId)
                .Sum(a => a.Amount);
            return profit;
        }

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



