import { IApartment, ITransaction } from "./basic";

export interface IRenovationLine {
  id: number;
  title: string;
  category: number;
  cost: number;
  //totalPrice: number;
  comments: string;
  renovationProject: IRenovationProject;
  isCompleted: boolean;
}
export interface IRenovationProject {
  id: number;
  name: string;
  //cost: number;
  comments: string;
  dateStart: Date;
  dateEnd: Date;
  apartment: IApartment;
  peneltyPerDay: number;
  transactionId: number;
}

export interface IRenovationPayment {
  id: number;
  title: string;
  amount: number;
  //cost: number;
  criteria: string;
  comments: string;
  datePayment: Date;
  renovationProjectId: number;
  renovationProject: IRenovationProject;
  // checkIdWriten: boolean;
  // checkInvoiceScanned: boolean;
  isConfirmed: boolean;
  
}

export interface IRenovationProduct {
  id: number;
  name: string;
  description: string;
  store: string;
  price: number;
  photoUrl: string;
  link: string;
  serialNumber: string;

}







// export interface ILine {
//   id: number;
//   title: Date;
//   category: number;
//   workCost: boolean;
//   itemsTotalPrice: boolean;
//   totalPrice: boolean;
//   comments: number;
//   items: IItem[];

// }
// export interface IItem {
//   id: number;
//   description: string;
//   quantity: number;
//   product: IProduct;
// }
// export interface IProduct {
//   id: number;
//   name: string;
//   price: number;
//   reference: string;
//   link: string;

// }

// export interface IItemDto {
//   itemId: number;
//   lineId: number;
//   lineTitle: string;
//   lineCategory: number;
//   itemDescription: string;
//   productId: number;
//   productName: string;
//   quantity: number;
//   price: number;
//   totalPrice: number;
//   reference: string;
//   link: string;

// }
