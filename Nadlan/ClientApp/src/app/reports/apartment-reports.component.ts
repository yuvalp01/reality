import { Component, OnInit, Input, EventEmitter, Output, SimpleChanges, ViewEncapsulation, Inject } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction, IApartment } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { filter, debounce } from 'rxjs/operators';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { TransactionsDialogComponent } from '../transactions/transactions-dialog.component';
import { ProviderAst } from '@angular/compiler';
import { debug } from 'util';

@Component({
  selector: 'apartment-reports',
  templateUrl: './apartment-reports.component.html',
  styleUrls: ['./apartment-reports.component.css'],
  //encapsulation: ViewEncapsulation.ShadowDom
})
export class ApartmentReportsComponent implements OnInit {
  displayedColumns: string[] = ['date', 'amount', 'apartmentId', 'accountId', 'isPurchaseCost', 'comments'];
  incomeReport: IIncomeReport;
  purchaseReport: IPurchaseReport;
  summaryReport: ISummaryReport;
  apartmentInfo: IApartment;
  transactions: ITransaction[];
  years: number[] = [2018, 2019];
  selectedYear: number = 0;
  @Input() apartmentId: number = 0;
  dataSource = new MatTableDataSource<ITransaction>();

  @Output() myEvent = new EventEmitter();


  //simulation params:
  rentMonthsInYear: number = 11;
  buyerExpectedRerutn: number = 0.05;

  constructor(private reportsService: ReportService,
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }
  ngOnInit(): void {
    debugger
    if (this.data.apartmentId) {
      this.apartmentId = +this.data.apartmentId;
      this.loadApartmentReports(this.apartmentId);
    }

    this.route.paramMap.subscribe(params => {
      this.apartmentId = +params.get('apartmentId');
      this.loadApartmentReports(this.apartmentId);
    });



  }


  ngOnChanges(changes: SimpleChanges): void {
    //console.log('changes :' + changes.apartmentId.previousValue);
    //console.log('changes :' + changes.apartmentId.currentValue);
    this.apartmentId = changes.apartmentId.currentValue;
    this.loadApartmentReports(this.apartmentId);

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
    this.transactionService.getTransactionsByAccount(this.apartmentId, accountId, isPurchaseCost, this.selectedYear).subscribe(
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


  onChange(e) {
    this.reportsService.getIncomeReport(this.apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
  }

  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }
}
