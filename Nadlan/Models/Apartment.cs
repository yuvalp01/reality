using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Apartment
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public short Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Floor { get; set; }
        public int Size { get; set; }
        public string Door { get; set; }
        public decimal CurrentRent { get; set; }
        public decimal FixedMaintanance { get; set; }

    }


}
