import { Component, OnInit, Input, EventEmitter, Output, SimpleChanges, Inject } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction, IApartment } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { TransactionsDialogComponent } from '../transactions/transactions-dialog.component';


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
  apartmentInfo: IApartment;
  //transactions: ITransaction[];
  years: number[] = [2018, 2019, 2020];
  selectedYear: number = 0;
  @Input() apartmentId: number = 0;
  dataSource = new MatTableDataSource<ITransaction>();
  isIgnoreChanges: boolean = true;
  @Output() myEvent = new EventEmitter();
  //role: number;

  //simulation params:
  rentMonthsInYear: number = 11;
  buyerExpectedRerutn: number = 0.05;

  @Input() investorPercentage: number = null;
  percentage: number = 1;
  showPercentage: boolean = false;

  constructor(private reportsService: ReportService,
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }
  ngOnInit(): void {

    //this.role = +window.sessionStorage.getItem("role");

    //when open as a dialog
    if (this.data.apartmentId) {
      this.apartmentId = +this.data.apartmentId;
      this.investorPercentage = +this.data.investorPercentage;
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
      this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
      this.reportsService.getSummaryReport(apartmentId).subscribe(result => this.summaryReport = result, error => console.error(error));
      this.reportsService.getIncomeReport(apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
      this.reportsService.getApartmentInfo(apartmentId).subscribe(result => this.apartmentInfo = result, error => console.error(error));
    }
  }


  showTrans(accountId, accountName, isPurchaseCost) {
    let year = this.selectedYear;
    //Purchase costs and distribution are not year dependant
    if (isPurchaseCost || accountId == 100) {
      year = 0;
    }
    this.transactionService.getTransactionsByAccount(this.apartmentId, accountId, isPurchaseCost, year).subscribe(
      result => {
        //this.transactions = result;
        this.dataSource.data = result as ITransaction[];
        const dialogRef = this.dialog.open(TransactionsDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, accountName: accountName }
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

  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }
}
