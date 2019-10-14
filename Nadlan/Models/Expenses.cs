using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public decimal Hours { get; set; }
        //[NotMapped]
        //public int AppartmentId { get; set; }
        //[NotMapped]
        //public int AccountId { get; set; }
        //[NotMapped]
        //public string Comments { get; set; }
    }
}

