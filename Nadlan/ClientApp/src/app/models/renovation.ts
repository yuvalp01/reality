
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

export interface IItemDto {
  itemId: number;
  lineId: number;
  lineTitle: string;
  lineCategory: number;
  itemDescription: string;
  productId: number;
  productName: string;
  quantity: number;
  price: number;
  totalPrice: number;
  reference: string;
  link: string;

}
