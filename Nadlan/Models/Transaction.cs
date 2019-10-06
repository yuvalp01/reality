using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsPurchaseCost { get; set; }
        public string Comments { get; set; }
        public bool IsBusinessExpense { get; set; }      
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
