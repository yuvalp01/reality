using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Portfolio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public int StakeholderId { get; set; }
        public Stakeholder Stakeholder { get; set; }
        //public int AccountId { get; set; }
        //public Account Account { get; set; }
        public decimal Percentage { get; set; }
    }
}
