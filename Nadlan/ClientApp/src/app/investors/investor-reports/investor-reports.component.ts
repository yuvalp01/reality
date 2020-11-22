import { Component, OnInit, SimpleChanges, Output } from "@angular/core";
import { IInvestorReportOverview, IStakeholder, IPersonalTransaction, ITransaction, IPortfolioReport } from "../../models";
import { PersonalTransService } from "../personal-trans.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource, MatDialog } from "@angular/material";
import { PersonalTransDialogComponent } from "../personal-trans-dialog/personal-trans-dialog.component";
import { ApartmentReportsComponent } from "../../reports/apartment-reports.component";
import { debug } from "util";
import { TransactionsDialogComponent } from "../../transactions/transactions-dialog.component";
import { TransactionService } from "../../services/transaction.service";
import { SecurityService } from "../../security/security.service";

@Component({
  selector: 'investor-reports',
  templateUrl: './investor-reports.component.html',
  styleUrls: ['./investor-reports.component.css'],

})
export class InvestorReportComponent implements OnInit {

  constructor(
    private personalTransService: PersonalTransService,
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private securityService: SecurityService,
    private dialog: MatDialog) { }
  @Output() stakeholderId: number;
  investorReportOverview: IInvestorReportOverview;
  portfolioReport: IPortfolioReport[];
  stakeholders: IStakeholder[];
  portfolioColumns = ['apartment', 
  'purchaseDate', 
  'investment',
  //  'distributed',
  //  'pendingProfits',
  //   'pendingExpenses',
  //    'profitsSoFar',
  //    'pendingBonus',
     'netProfit',
      'ownership'];
  selectedTab: number;
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.stakeholderId = +params.get("stakeholderId");
      this.refreshData(this.stakeholderId);
    });

  }

  ngOnChanges(changes: SimpleChanges): void {
    this.stakeholderId = changes.stakeholderId.currentValue;
    this.refreshData(this.stakeholderId);
  }

  refreshData(stakeholderId: number) {
    this.investorReportOverview = null;
    this.personalTransService.getInvestorReport(stakeholderId)
      .subscribe(result => this.investorReportOverview = result, error => console.error(error));

    this.personalTransService.getPortfoio(stakeholderId)
      .subscribe(result => this.portfolioReport = result, error => console.error(error));
      
    this.loadAllStakeholders();
  }


  loadAllStakeholders() {
    this.personalTransService.getStakeholders().subscribe({
      next: (result) => this.stakeholders = result,
      error: (error) => console.error(error)
    })
  }

  dataSource = new MatTableDataSource<IPersonalTransaction>();
  showDistributions() {
    this.personalTransService.getAllDistributions(this.stakeholderId).subscribe(
      result => {
        //this.transactions = result;
        this.dataSource.data = result as IPersonalTransaction[];
        const dialogRef = this.dialog.open(PersonalTransDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, columns: ['date', 'amount', 'transactionType', 'comments', 'apartment'] }
        });
      }
      , error => console.error(error));

  }
  showApartmentReport(apartmentId, ownership) {

    const dialogRef = this.dialog.open(ApartmentReportsComponent, {
      height: 'auto',
      width: 'auto',
      data: { apartmentId: apartmentId, investorPercentage: ownership }
    });
  }


  showPendingExpenses() {
    if (this.securityService.hasClaim('admin')) {
      this.transactionService.getPendingExpensesForInvestor(this.stakeholderId).subscribe(
        result => {
          //this.transactions = result;
          let _transactions = result as ITransaction[];
          const dialogRef = this.dialog.open(TransactionsDialogComponent, {
            height: 'auto',
            width: 'auto',
            data: { transactions: _transactions, accountName: "PendingExpenses" }
          });
        }
        , error => console.error(error));
    }
  }


  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }


}
