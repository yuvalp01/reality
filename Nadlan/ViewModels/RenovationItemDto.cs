using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.ViewModels
{
    public class RenovationItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Category Category { get; set; }
        public decimal WorkCost { get; set; }
        public string Comments { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }

        public int ApartmentId { get; set; }
    }
}
