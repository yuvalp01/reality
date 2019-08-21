
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

export interface IIncomeReport {
  grossIncome:number,
  expenses: number,
  tax:number,
  netIncome: number,
  forDistribution: number,
}

export interface IPurchaseReport {
  investment: number,
  totalCost: number,
  renovationCost: number,
  expensesNoRenovation: number,
  remainder:number,
  accountsSum: IAccountsSum,
 
}
export interface IAccountsSum {
  accountId: number,
  name: number,
  total: number,

}

