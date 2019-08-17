
export interface ITransaction {
    id:number;
    date: Date;
    comments: string;
    isPurchaseCost: boolean;
    amount: number;
    apartmentId:number;
    apartmentAddress:string;
    accountId:number;
    accountName:string;
  }


 export interface IApartment {
    id: number; 
    address: string;
    status: number;
  }

  export interface IAccount {
    id: number;
    name: string;
    isIncome: boolean;
  }