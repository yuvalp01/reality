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

        public ICollection<Transaction> Transactions { get; set; }
    }
}
