using Nadlan.Models.Security;
using Newtonsoft.Json;
using System;

namespace Nadlan.Models.Issues
{
    public class Message
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateStamp { get; set; }

        //public Guid UserId { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
        public int IssueId { get; set; }
        [JsonIgnore]
        public virtual Issue Issue { get; set; }

        public bool IsDeleted { get; set; }

    }
}
