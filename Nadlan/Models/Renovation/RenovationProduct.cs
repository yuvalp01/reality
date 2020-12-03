using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Renovation
{

    [Table("products", Schema = "renovation")]
    public class RenovationProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Store { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public string Link { get; set; }
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public string ItemType { get; set; }
    }


}
