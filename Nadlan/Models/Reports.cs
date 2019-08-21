using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class PurchaseReport
    {
        public decimal Investment { get; set; }
        public decimal TotalCost { get; set; }
        public decimal RenovationCost { get; set; }
        public decimal ExpensesNoRenovation { get; set; }
        public decimal Remainder{ get; set; }
        public IEnumerable<AccountSummary> AccountsSum { get; set; }
    }

    public class SummaryReport
    {
        public decimal GrossIncome { get; set; }
        public decimal Expenses { get; set; }
        public decimal Tax { get; set; }
        public decimal NetIncome { get; set; }
        public decimal ForDistribution { get; set; }
    }

    public class AccountSummary
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    }
}
