import { Component, OnInit, Input, EventEmitter, Output, SimpleChanges, Inject } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction, IApartment, IEndOfTheYearCalc, ISoFarReport, IFilter, IPersonalTransaction } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { TransactionsDialogComponent } from '../transactions/transactions-dialog.component';
import { TransactionFormComponent } from '../transactions/transaction-form/transaction-form.component';
import { PersonalTransFormComponent } from '../investors/personal-trans-form/personal-trans-form.component';


@Component({
  selector: 'apartment-reports',
  templateUrl: './apartment-reports.component.html',
  styleUrls: ['./apartment-reports.component.css'],
})
export class ApartmentReportsComponent implements OnInit {
  displayedColumns: string[] = ['date', 'amount', 'apartmentId', 'accountId', 'isPurchaseCost', 'comments'];
  incomeReport: IIncomeReport;
  purchaseReport: IPurchaseReport;
  summaryReport: ISummaryReport;
  soFarReport: ISoFarReport;
  apartmentInfo: IApartment;
  buttonCalcClicked: boolean = false;
  years: number[] = [2018, 2019, 2020, 2021, 2022, 2023];
  partnershipApartments: number[] = [1, 3, 4, 20];
  selectedYear: number = 0;
  @Input() apartmentId: number = 0;
  dataSource = new MatTableDataSource<ITransaction>();
  isIgnoreChanges: boolean = true;
  @Output() myEvent = new EventEmitter();
  math = Math;
  filter: IFilter;

  //simulation params:
  rentMonthsInYear: number = 11;
  buyerExpectedRerutn: number = 0.05;

  @Input() investorPercentage: number = null;
  percentage: number = 1;
  showPercentage: boolean = true;

  constructor(private reportsService: ReportService,
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.filter = {} as IFilter;
  }
  ngOnInit(): void {

    //when open as a dialog
    if (this.data.apartmentId) {
      this.apartmentId = +this.data.apartmentId;
      this.investorPercentage = +this.data.investorPercentage;
      this.percentage = this.investorPercentage;
      this.loadApartmentReports(this.apartmentId);
      this.isIgnoreChanges = false;
    }
    else {
      this.route.paramMap.subscribe(params => {
        let _apartmentId = +params.get('apartmentId');
        this.apartmentId = _apartmentId;
        this.loadApartmentReports(this.apartmentId);
      });
    }
  }


  ngOnChanges(changes: SimpleChanges): void {
    if (!this.isIgnoreChanges) {
      this.apartmentId = changes.apartmentId.currentValue;
      this.loadApartmentReports(this.apartmentId);
    }


  }


  loadApartmentReports(apartmentId: number) {
    if (apartmentId) {
      this.summaryReport = null;
      this.purchaseReport = null;
      this.incomeReport = null;
      this.apartmentInfo = null;
      this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
      this.reportsService.getSummaryReport(apartmentId, this.selectedYear).subscribe(result => this.summaryReport = result, error => console.error(error));
      this.reportsService.getIncomeReport(apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
      this.reportsService.getApartmentInfo(apartmentId).subscribe(result => this.apartmentInfo = result, error => console.error(error));
    }
  }


  loadTransactionNew(accountId: number) {
    this.filter.apartmentId = this.apartmentId;
    this.filter.year = this.selectedYear;
    this.filter.accountId = accountId;
    this.filter.isSoFar = true;

    this.transactionService.getFilteredTransactions(this.filter).subscribe({
      next: (result) => {
        this.dataSource.data = result as ITransaction[];
        const dialogRef = this.dialog.open(TransactionsDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, accountName: 'Transactions up-to-date' }
        });
      },
      error: (err) => console.error(err)
    });
  }

  showTrans(accountId, accountName, isPurchaseCost) {
    let year = this.selectedYear;
    //Purchase costs and distribution are not year dependant
    if (isPurchaseCost || accountId == 100) {
      year = 0;
    }
    this.transactionService.getTransactionsByAccount(this.apartmentId, accountId, isPurchaseCost, year).subscribe(
      result => {
        this.dataSource.data = result as ITransaction[];
        const dialogRef = this.dialog.open(TransactionsDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, accountName: accountName }
        });
      }
      , error => console.error(error));

  }

  showPendingExpenses() {
    this.transactionService.getPendingExpensesForApartment(this.apartmentId, this.selectedYear).subscribe(
      result => {
        this.dataSource.data = result as ITransaction[];
        const dialogRef = this.dialog.open(TransactionsDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, accountName: "Pending Expenses" }
        });
      }
      , error => console.error(error));

  }

  showHidePercentage() {
    this.showPercentage = !this.showPercentage;
    if (this.showPercentage) {
      this.percentage = this.investorPercentage;
    }
    else {
      this.percentage = 1;;
    }
  }
  onChange(e) {
    this.reportsService.getIncomeReport(this.apartmentId, this.selectedYear)
      .subscribe(result => this.incomeReport = result, error => console.error(error));
  }


  calcSoForReport() {
    this.soFarReport = null;
    this.buttonCalcClicked = true;
    this.reportsService.GetSoFarReport(this.apartmentId, this.selectedYear).subscribe({
      next: (result) => { this.soFarReport = result; this.buttonCalcClicked = false },
      error: err => console.error(err)
    });
  }


  openPersonalTransactionForm() {
    if (Math.floor(this.soFarReport.pendingBonus) !== 0) {
      alert('Distribute bonus first and calculate again');
    }
    else {
      let personalTransaction: IPersonalTransaction = {} as IPersonalTransaction;
      personalTransaction.amount = this.soFarReport.pendingExpenses;
      personalTransaction.apartmentId = this.apartmentId;
      personalTransaction.date = new Date(this.selectedYear, 11, 31);
      personalTransaction.transactionType = 10;
      personalTransaction.comments = `Expenses and compensation ${this.selectedYear}`;
      let dialogRef = this.dialog.open(PersonalTransFormComponent, {
        height: '600px',
        width: '500px',
        data: { transactionId: 0, expected: personalTransaction }
      });
      dialogRef.componentInstance.refreshEmitter.subscribe(() => {
        this.calcSoForReport();
      });
    }

  }


  openTransactionForm(transactionType) {

    let transaction: ITransaction = this.buildTransaction(transactionType);
    let dialogRef = this.dialog.open(TransactionFormComponent, {
      height: '600px',
      width: '500px',
      data: { transactionId: 0, expected: transaction }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => {
      this.calcSoForReport();
    });
    // }
  }


  buildTransaction(transactionType: string): ITransaction {
    let expectedTran = {} as ITransaction;

    if (transactionType == 'distribution') {
      expectedTran.accountId = 100;
      expectedTran.amount = this.soFarReport.pendingDistribution;
      expectedTran.comments = `End of the year distribution ${this.selectedYear} - ${this.apartmentInfo.address}`;
      expectedTran.personalTransactionId = -2;
    }
    else if (transactionType == 'bonus') {
      expectedTran.accountId = 300;
      expectedTran.amount = this.soFarReport.pendingBonus;
      expectedTran.comments = `End of the year compensation ${this.selectedYear} - ${this.apartmentInfo.address}`;
      if (this.partnershipApartments.includes(this.apartmentId)) {
        expectedTran.personalTransactionId = -2;
      }
      else {
        expectedTran.personalTransactionId = 0;
      }

    }
    expectedTran.id = 0;
    expectedTran.apartmentId = this.apartmentId;
    expectedTran.date = new Date(this.selectedYear, 11, 31);
    expectedTran.isBusinessExpense = false;
    expectedTran.isConfirmed = false;
    expectedTran.isPurchaseCost = false;


    return expectedTran;
  }


  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }
}
