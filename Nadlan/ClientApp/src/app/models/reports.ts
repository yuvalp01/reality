
export interface ISummaryReport {
  investment: number,
  netIncome: number,
  roi: number,
  roiForInvestor: number,
  predictedROI: number;
  initialRemainder: number,
  balance: number,
  distributed: number,
  bonusExpected: number,
  bonusPaid: number,
  years: number,
  roiAccumulated: number,
  thresholdAccumulated: number,
  bonusSoFar: number,
  bonusPercentage: number,
  netForInvestor: number,

}
export interface ISoFarReport {
  investment: number,
  netIncome: number,
  grossIncome: number,
  pendingExpenses: number,
  roi: number,
  roiForInvestor: number,
  predictedROI: number;

  pendingDistribution: number,
  distributed: number,

  Bonus: number,
  bonusPaid: number,
  pendingBonus: number,
  bonusPercentage: number,

  years: number,
  roiAccumulated: number,
  thresholdAccumulated: number,
  netForInvestor: number,


}


export interface IIncomeReport {
  grossIncome: number,
  expenses: number,
  tax: number,
  netIncome: number,
  //forDistribution: number,
  netForInvestor: number,
  accountsSum: IAccountsSum,
  bonusExpected: number,
  bonusPaid: number,
  bonus: number,
}

export interface IPurchaseReport {
  investment: number,
  totalCost: number,
  renovationCost: number,
  expensesNoRenovation: number,
  remainder: number,
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
  totalNetProfit: number,


  cashBalance: number,
  totalPendingProfits: number,
  totalPendingBonus: number,
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
  netProfit: number,


  pendingProfits: number,
  pendingExpenses: number,
  ownership: number,
  //[Obsolete]
  minimalProfitUpToDate: number,
  //[Obsolete]
  distributed: number,

}


