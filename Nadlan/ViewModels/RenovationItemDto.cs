using Nadlan.Models.Renovation;

namespace Nadlan.ViewModels.Renovation
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public int LineId { get; set; }
        public string LineTitle { get; set; }
        public RenovationCategory LineCategory { get; set; }
        public string ItemDescription { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Reference { get; set; }
        public string Link { get; set; }



        //public int ApartmentId { get; set; }
    }
}
