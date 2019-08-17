using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class RenovationLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal WorkCost { get; set; }
        public string Comments { get; set; }

        public RenovationItem RenovationItem { get; set; }
        public int RenovationItemId { get; set; }
        public  Apartment Apartment { get; set; }
        public int ApartmentId { get; set; }
    }
}
