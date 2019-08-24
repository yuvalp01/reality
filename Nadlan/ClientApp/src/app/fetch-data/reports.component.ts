import {Component, OnInit, Output} from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport } from '../shared/models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
    templateUrl:'./reports.component.html',
    styles:['table{width:100%}']
})
export class ReportsComponent implements OnInit
{
    displayedColumns: string[] = ['date','amount', 'apartmentId','accountId','isPurchaseCost','comments'];
    incomeReport: IIncomeReport;
    purchaseReport: IPurchaseReport;
    summaryReport: ISummaryReport;
    years: number[] = [2018,2019];
    selectedYear: number = 0;
    apartmentId: number = 0;
  constructor(private reportsService: ReportService, private route: ActivatedRoute)
    {
    }
  ngOnInit(): void {
    this.apartmentId = this.route.snapshot.params['apartmentId'];
    if (this.apartmentId) {
      console.log(this.apartmentId);
      this.reportsService.getPurchaseReport(this.apartmentId).subscribe(result => this.purchaseReport = result, error => console.error(error));
      this.reportsService.getSummaryReport(this.apartmentId).subscribe(result => this.summaryReport = result, error => console.error(error));
      this.reportsService.getIncomeReport(this.apartmentId, this.selectedYear).subscribe(result => {
        this.incomeReport = result;
        //this.ROI = this.incomeReport.netIncome / this.purchaseReport.investment;

        //const date1 = new Date('7/13/2010');
        //const date2 = new Date('12/15/2010');
        //const diffTime = Math.abs(date2.getTime() - date1.getTime());
        //const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24*12));
        //console.log(diffDays);


    }, error => console.error(error));
    }


    //this.route.events.pipe(
    //  filter((event: RouterEvent) => event instanceof NavigationEnd)
    //).subscribe(() => {
    //this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport=result, error=>console.error(error));
    //this.reportsService.getIncomeReports(apartmentId, 2018).subscribe(result => this.incomeReport = result, error => console.error(error));
    //});

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
