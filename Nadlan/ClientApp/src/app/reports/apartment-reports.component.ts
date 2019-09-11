import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { TransactionsDialogComponent } from '../transactions/transactions-dialog.component';

@Component({
  selector: 'apartment-reports',
  templateUrl: './apartment-reports.component.html',
  styles: ['table{width:100%}']
})
export class ApartmentReportsComponent implements OnInit {
  displayedColumns: string[] = ['date', 'amount', 'apartmentId', 'accountId', 'isPurchaseCost', 'comments'];
  incomeReport: IIncomeReport;
  purchaseReport: IPurchaseReport;
  summaryReport: ISummaryReport;
  transactions: ITransaction[];
  years: number[] = [2018, 2019];
  selectedYear: number = 0;
  //apartmentId: number = 1;
  @Input() apartmentId: number = 0;
  dataSource = new MatTableDataSource<ITransaction>();

  @Output() myEvent = new EventEmitter();

  constructor(private reportsService: ReportService, private transactionService: TransactionService, private route: ActivatedRoute, private dialog: MatDialog) {
  }
  ngOnInit(): void {
    //this.apartmentId = this.route.snapshot.params['apartmentId'];
    if (this.apartmentId) {
      this.reportsService.getPurchaseReport(this.apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
      this.reportsService.getSummaryReport(this.apartmentId).subscribe(result => this.summaryReport = result, error => console.error(error));
      this.reportsService.getIncomeReport(this.apartmentId, this.selectedYear).subscribe(result => {
        this.incomeReport = result;
      }, error => console.error(error));
    }


    //this.route.events.pipe(
    //  filter((event: RouterEvent) => event instanceof NavigationEnd)
    //).subscribe(() => {
    //this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport=result, error=>console.error(error));
    //this.reportsService.getIncomeReports(apartmentId, 2018).subscribe(result => this.incomeReport = result, error => console.error(error));
    //});

  }

  loadApartmentReports(apartmentId: number) {
    if (apartmentId) {
      this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
      this.reportsService.getSummaryReport(apartmentId).subscribe(result => this.summaryReport = result, error => console.error(error));
      this.reportsService.getIncomeReport(apartmentId, this.selectedYear).subscribe(result => {
        this.incomeReport = result;
      }, error => console.error(error));
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
