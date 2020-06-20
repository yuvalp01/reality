using Nadlan.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public class Message_
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateStamp { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
        public TypeMessage TypeMessage { get; set; }
        public int ItemId { get; set; }
        public bool IsDeleted { get; set; }

    }
   public enum TypeMessage
    {
        Issue = 10,
        Investor = 20,
        //Apartment = 30
    }
}
