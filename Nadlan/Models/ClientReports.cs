using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Reports
{
    public class InvestorReportOverview
    {
        public decimal TotalInvestment { get; set; }
        public decimal CashBalance { get; set; }
        public decimal MinimalProfitUpToDate { get; set; }
        public decimal TotalBalace { get; set; }
    }
    public class PortfolioReport
    {
        public string Apartment { get; set; }
        public decimal Investment { get; set; }
        public decimal Ownership { get; set; }
    }






}
