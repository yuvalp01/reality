
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
  Amount: number;
  date: Date;
  Comments: string;
  StakeholderId: number;

}

export interface IStakeholder {
  id: number;
  name: string;
  type: number;
}

