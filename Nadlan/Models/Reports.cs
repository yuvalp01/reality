using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nadlan.Models
{

    public class SummaryReport
    {
        public decimal Investment { get; set; }
        public decimal NetIncome { get; set; }
        public decimal ROI { get; set; }
        public decimal PredictedROI { get; set; }
        public decimal InitialRemainder { get; set; }
        public decimal Balance { get; set; }
        public decimal Distributed { get; set; }

    }

    public class PurchaseReport
    {
        public decimal Investment { get; set; }
        public decimal TotalCost { get; set; }
        public decimal RenovationCost { get; set; }
        public decimal ExpensesNoRenovation { get; set; }
        public decimal Remainder{ get; set; }
        public IEnumerable<AccountSummary> AccountsSum { get; set; }
    }

    public class IncomeReport
    {
        public decimal GrossIncome { get; set; }
        public decimal Expenses { get; set; }
        public decimal Tax { get; set; }
        public decimal NetIncome { get; set; }
        //public decimal ForDistribution { get; set; }
        public IEnumerable<AccountSummary> AccountsSum { get; set; }
    }

    public class AccountSummary
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    }

    public class EvaluationReport
    {
        public decimal TotalInvestment { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Evaluation { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ROI { get; set; }
    }

    public class DiagnosticRequest
    {
        public decimal Price { get; set; }
        public decimal Renovation { get; set; }
        public decimal PredictedRent { get; set; }
        public decimal KnowExtraCosts { get; set; }
        public int Size { get; set; }
    }


    public class DiagnosticReport
    {
        public decimal Legal { get; set; }
        public decimal Research { get; set; }
        public decimal PurchaseTax { get; set; }
        public decimal Agency { get; set; }
        public decimal Accountency { get; set; }
        public decimal Supervision { get; set; }
        public decimal Registration { get; set; }
        public decimal Unpredicted { get; set; }
        public decimal UnpredictedRenovation { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ROI { get; set; }
    }






}
