
export interface ISummaryReport {
  investment: number,
  netIncome: number,
  roi: number,
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
  totalInvestment: number,
  cashBalance: number,
  minimalProfitUpToDate: number,
  totalBalace: number,
  totalDistribution: number,
  portfolioLines: IPortfolioReport[],

}

export interface IPortfolioReport {
  apartment: string,
  purchaseDate: Date,
  investment: number,
  ownership: number,
  minimalProfitUpToDate: number,
  distributed: number,

}
//public class InvestorReportOverview {
//  public decimal TotalInvestment { get; set; }
//        public decimal CashBalance { get; set; }
//        public decimal MinimalProfitUpToDate { get; set; }
//        public decimal TotalBalace { get; set; }
//        public decimal TotalDistribution { get; set; }
//        public List < PortfolioReport > PortfolioLines { get; set; }
//    }
//public class PortfolioReport {
//  public string Apartment { get; set; }
//        public DateTime PurchaseDate { get; set; }
//        public decimal Investment { get; set; }
//        public decimal Ownership { get; set; }  
//        public decimal MinimalProfitUpToDate { get; set; }
//        public decimal Distributed { get; set; }


//    }

