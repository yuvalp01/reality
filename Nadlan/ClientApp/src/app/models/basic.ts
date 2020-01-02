
export interface ITransaction {
  id: number;
  date: Date;
  comments: string;
  isPurchaseCost: boolean;
  amount: number;
  apartmentId: number;
  apartmentAddress: string;
  accountId: number;
  accountName: string;
  isBusinessExpense: boolean;
  isConfirmed: boolean;
  hours: number;
  isCoveredByInvestor: boolean;
}


export interface IApartment {
  id: number;
  address: string;
  status: number;
  size: number;
  floor: number;
  door: number;
  currentRent: number;
  purchaseDate: Date;

}

export interface IAccount {
  id: number;
  name: string;
  isIncome: boolean;
  accountTypeId: number;
}
export interface IPersonalTransaction {
  id: number; 
  date: Date;
  amount: number;
  comments: string;
  stakeholderId: number;
  apartment: IApartment;

}

export interface IStakeholder {
  id: number;
  name: string;
  type: number;
}

