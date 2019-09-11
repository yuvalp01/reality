import { Component, OnInit, Output, Input } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { TransactionsDialogComponent } from '../transactions/transactions-dialog.component';
import { ApartmentReportsComponent } from './apartment-reports.component';

@Component({
  templateUrl: './reports.component.html',
  styles: ['table{width:100%}']
})
export class ReportsComponent implements OnInit {
  constructor(private reportsService: ReportService,  private route: ActivatedRoute) {
  }
  selectedApartmentId: number;
  ngOnInit(): void {
    this.selectedApartmentId = this.route.snapshot.params['apartmentId'];
    //this.selectedApartmentId = 1;

  }
  showReport(apartmentId: number) {
    //this.selectedApartmentId = apartmentId;
    this.selectedApartmentId = this.route.snapshot.params['apartmentId'];
    //this.com.loadApartmentReports(this.selectedApartmentId);
    //if (apartmentId) {
    //  this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
    //  this.reportsService.getSummaryReport(apartmentId).subscribe(result => this.summaryReport = result, error => console.error(error));
    //  this.reportsService.getIncomeReport(apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
    //}


  }
}
