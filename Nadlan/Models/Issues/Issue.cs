using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models.Issues
{
    [Table("issues")]
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime? DateClose { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public bool IsNew { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public virtual ICollection<Message> Messages { get; set; }
    }


}
