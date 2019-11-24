using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("personalTransactions")]
    public class PersonalTransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public TransactionType TransactionType { get; set; }
        public int StakeholderId { get; set; }
        public Stakeholder Stakeholder { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
    }
}
