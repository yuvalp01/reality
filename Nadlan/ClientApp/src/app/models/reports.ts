
export interface ISummaryReport {
  investment: number,
  netIncome: number,
  roi: number,
  predictedROI: number;
  initialRemainder: number,
  balance: number,
}


export interface IIncomeReport {
  grossIncome:number,
  expenses: number,
  tax:number,
  netIncome: number,
  forDistribution: number,
  accountsSum: IAccountsSum,
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

