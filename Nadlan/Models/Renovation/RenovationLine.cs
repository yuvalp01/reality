using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Renovation
{
    [Table("lines",Schema ="renovation")]
    public class RenovationLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RenovationCategory Category { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public  RenovationProject RenovationProject { get; set; }
        public int RenovationProjectId { get; set; }
        public bool IsCompleted { get; set; }
    }

    public enum RenovationCategory
    {
        General = 0,
        Kitchen = 1,
        Bathroom = 2,
        Room =3
    }

}
