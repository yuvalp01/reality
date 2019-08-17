using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class RenovationItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string link { get; set; }

        public ICollection<RenovationLine> Lines { get; set; }

    }
}
