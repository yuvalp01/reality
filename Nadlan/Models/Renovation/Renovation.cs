using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Renovation
{
    //[Table("Lines", Schema = "renovation")]
    [Obsolete]
    public class Line
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RenovationCategory Category { get; set; }
        public decimal WorkCost { get; set; }
        public string Comments { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
        [NotMapped]
        public decimal ItemsTotalPrice { get; set; }
        [NotMapped]
        public decimal TotalPrice { get; set; }
        public Apartment Apartment { get; set; }
        public int ApartmentId { get; set; }
    }

    //public enum RenovationCategory
    //{
    //    General = 0,
    //    Kitchen = 1,
    //    Bathroom = 2,
    //    Room = 3
    //}
    [Obsolete]
    [Table("Items", Schema = "renovation")]
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
    [Obsolete]
    [Table("Products", Schema = "renovation")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Reference { get; set; }
        public string Link { get; set; }

    }

}
