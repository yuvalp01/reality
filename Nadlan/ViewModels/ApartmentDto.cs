using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.ViewModels
{
    public class ApartmentDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Floor { get; set; }
        public int Size { get; set; }
        public string Door { get; set; }
        public decimal CurrentRent { get; set; }
    }
}
