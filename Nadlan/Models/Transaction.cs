using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool IsConfirmed { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        [NotMapped]
        public decimal Hours { get; set; }
    }
}
