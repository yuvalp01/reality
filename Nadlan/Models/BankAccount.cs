using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int StakeholderId { get; set; }
        public string Name { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string UserNameBank { get; set; }
        public string LinkToWebsite { get; set; }
    }
}
