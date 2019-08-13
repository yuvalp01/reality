using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
