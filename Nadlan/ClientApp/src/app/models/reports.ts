
export interface ISummaryReport {
  investment: number,
  netIncome: number,
  roi: number,
  roiForInvestor:number,
  predictedROI: number;
  initialRemainder: number,
  balance: number,
  distributed: number,
}


export interface IIncomeReport {
  grossIncome:number,
  expenses: number,
  tax:number,
  netIncome: number,
  //forDistribution: number,
  bonus: number,
  netForInvestor: number,
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

export interface IInvestorReportOverview {
  name: string,
  totalInvestment: number,
  cashBalance: number,
  totalPendingProfits: number,
  totalPendingExpenses: number,
  totalBalace: number,
  portfolioLines: IPortfolioReport[],
  totalDistribution: number,
  //obsolete
  minimalProfitUpToDate: number,
}

export interface IPortfolioReport {
  apartmentId: string,
  apartment: string,
  purchaseDate: Date,
  investment: number,
  pendingProfits: number,
  pendingExpenses: number, 
  ownership: number,
  //[Obsolete]
  minimalProfitUpToDate: number,
  //[Obsolete]
  distributed: number,

}


