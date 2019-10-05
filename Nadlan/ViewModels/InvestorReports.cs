using System;
using System.Collections.Generic;

namespace Nadlan.ViewModels.Reports
{
    public class InvestorReportOverview
    {
        public decimal TotalInvestment { get; set; }
        public decimal CashBalance { get; set; }
        public decimal MinimalProfitUpToDate { get; set; }
        public decimal TotalBalace { get; set; }
        public decimal TotalDistribution { get; set; }
        public List<PortfolioReport> PortfolioLines { get; set; }
    }
    public class PortfolioReport
    {
        public int ApartmentId { get; set; }      
        public string Apartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Investment { get; set; }
        public decimal Ownership { get; set; }  
        public decimal MinimalProfitUpToDate { get; set; }
        public decimal Distributed { get; set; }


    }

    //public class PortfolioLineReport
    //{
    //    public string ApartmentName { get; set; }
    //    public decimal MinimalProfitUpToDate { get; set; }
    //}






}
