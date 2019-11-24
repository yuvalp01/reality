using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("expenses")]
    public class Expense
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public decimal Hours { get; set; }
    }
}

