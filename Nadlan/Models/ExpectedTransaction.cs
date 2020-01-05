using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("expectedTransactions")]
    public class ExpectedTransaction
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public int FrequencyPerYear { get; set; }
        public string Comment { get; set; }

    }

}
