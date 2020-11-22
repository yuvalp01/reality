using System;
using System.Collections.Generic;

namespace Nadlan.ViewModels.Reports
{
    public class InvestorReportOverview
    {
        public string Name { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalNetProfit { get; set; }
        /// <summary>
        /// If positive, this sum is yours and can be transferred to your bank account at any time
        /// </summary>
        public decimal CashBalance { get; set; }
        public decimal TotalPendingProfits { get; set; }
        public decimal TotalPendingExpenses { get; set; }
        public decimal TotalPendingBonus { get; set; }
        //public decimal ProfitsSoFar { get; set; }
        public decimal TotalBalace { get; set; }
        public List<PortfolioReport> PortfolioLines { get; set; }
        //[Obsolete]
        //public decimal MinimalProfitUpToDate { get; set; }
        //[Obsolete]
        public decimal TotalDistribution { get; set; }
    }
    public class PortfolioReport
    {
        public int ApartmentId { get; set; }
        public string Apartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Investment { get; set; }
        public decimal Ownership { get; set; }
        public decimal PendingProfits { get; set; }
        public decimal PendingExpenses { get; set; }
        public decimal PendingBonus { get; set; }
        /// <summary>
        /// PendingProfits - PendingExpenses - PendingBonus
        /// </summary>
        //public decimal NetPendingProfit { get; set; }
        public decimal NetProfit { get; set; }
        //[Obsolete]
        //public decimal MinimalProfitUpToDate { get; set; }
        //[Obsolete]
        public decimal Distributed { get; set; }


    }

}
