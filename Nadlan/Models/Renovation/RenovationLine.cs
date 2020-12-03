using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models.Renovation
{
    [Table("lines", Schema = "renovation")]
    public class RenovationLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RenovationCategory Category { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public int Units { get; set; }
        public RenovationProject RenovationProject { get; set; }
        public int RenovationProjectId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        public RenovationProduct Product { get; set; }
        public int ProductId { get; set; }

    }

    public enum RenovationCategory
    {
        General = 0,
        Kitchen = 1,
        Bathroom = 2,
        Room = 3
    }

}
