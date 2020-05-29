using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("transactions")]
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

        public bool IsCoveredByInvestor { get; set; }
        public int PersonalTransactionId { get; set; }


        //public int PaidById { get; set; }
        //public Stakeholder PaidBy { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        public decimal Hours { get; set; }
    }
}
