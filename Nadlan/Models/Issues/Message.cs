using Nadlan.Models.Security;
using System;

namespace Nadlan.Models.Issues
{
    public class Message
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateStamp { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public bool IsDeleted { get; set; }

    }
}
