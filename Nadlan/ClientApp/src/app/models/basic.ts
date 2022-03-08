import { IMessage } from ".";

export interface ITransaction {
  id: number;
  date: Date;
  comments: string;
  isPurchaseCost: boolean;
  amount: number;
  apartmentId: number;
  apartmentAddress: string;
  apartmentStatus: number;
  accountId: number;
  accountName: string;
  isBusinessExpense: boolean;
  isConfirmed: boolean;
  hours: number;
  personalTransactionId: number;
  messages: IMessage[];
  createdBy: number;
  isPettyCash: boolean;
  isPending: boolean;

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


export interface IContract {
  id: number;
  apartment: IApartment;
  tenant: string;
  tenantPhone: string;
  tenantEmail: string;
  dateStart: Date;
  dateEnd: Date;
  paymentDay: number;
  price: number;
  penaltyPerDay: number;
  deposit: number;
  link: string;
  isElectriciyChanged: boolean;
  conditions: string;
  isPaymentConfirmed: boolean;
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
  apartmentId: number;
  apartment: IApartment;
  transactionType: number;

}

export interface IStakeholder {
  id: number;
  name: string;
  type: number;
}

export interface IAppUser {
  userId: number;
  userName: string
}

export interface IFilter {
  accountId: number;
  apartmentId: number;
  monthsBack: number;
  isPurchaseCost: boolean;
  year: number;
  isSoFar: boolean;
  personalTransactionId: number;
  isLiteObject: boolean;
}

export interface IPersonalTransWithFilter {
  personalTransaction: IPersonalTransaction;
  filter: IFilter;
}