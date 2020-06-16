using Nadlan.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Issues
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority{ get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime? DateClose { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
    }

    public class IssueItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateStamp { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
    }
}
