
export interface ILine {
  id: number;
  title: Date;
  category: number;
  workCost: boolean;
  itemsTotalPrice: boolean;
  totalPrice: boolean;
  comments: number;
  items: IItem[];

}
export interface IItem {
  id: number;
  description: string;
  quantity: number;
  product: IProduct;
}
export interface IProduct {
  id: number;
  name: string;
  price: number;
  reference: string;
  link: string;

}

//public class RenovationLine {
//  public int Id { get; set; }
//        public string Title { get; set; }
//        public int Quantity { get; set; }
//        public decimal WorkCost { get; set; }
//        public string Comments { get; set; }

//        public RenovationItem RenovationItem { get; set; }
//        public int RenovationItemId { get; set; }
//        public Apartment Apartment { get; set; }
//        public int ApartmentId { get; set; }
//    }
