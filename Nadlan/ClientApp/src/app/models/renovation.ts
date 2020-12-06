import { NumberValueAccessor } from "@angular/forms";
import { IApartment, ITransaction } from "./basic";

export interface IRenovationLine {
  id: number;
  title: string;
  category: number;
  cost: number;
  units: number;
  //totalPrice: number;
  comments: string;
  renovationProject: IRenovationProject;
  renovationProjectId: number;
  isCompleted: boolean;
  isEditMode:boolean;
  productId:number;
  insertStage:number;
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
  stage:number
}

export interface IRenovationPayment {
  id: number;
  title: string;
  amount: number;
  criteria: string;
  comments: string;
  datePayment: Date;
  renovationProjectId: number;
  renovationProject: IRenovationProject;
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
  itemType: string;
  selectedItems: number;

}

