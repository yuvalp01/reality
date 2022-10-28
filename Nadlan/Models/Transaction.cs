using System;
using System.Collections.Generic;
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

        public int PersonalTransactionId { get; set; }

        public int CreatedBy { get; set; }
        public int BankAccountId { get; set; }
        
        public bool IsPending { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public List<Message> Messages { get; set; }

        //[NotMapped]
        public decimal Hours { get; set; }
    }
}
