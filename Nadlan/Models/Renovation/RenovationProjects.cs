using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Renovation
{
    [Table("projects", Schema = "renovation")]
    public class RenovationProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  Apartment Apartment { get; set; }
        public int ApartmentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        //public decimal AgreedPrice { get; set; }
        public decimal PeneltyPerDay { get; set; }
        public string Comments { get; set; }
        //public Transaction Transaction { get; set; }
        public int TransactionId { get; set; }
    }


}
