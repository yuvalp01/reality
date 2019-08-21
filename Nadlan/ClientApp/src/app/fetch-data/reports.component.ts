import {Component, OnInit, Output} from '@angular/core';
import { IIncomeReport, IPurchaseReport } from '../shared/models';
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
      this.reportsService.getIncomeReports(this.apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
    }


    //this.route.events.pipe(
    //  filter((event: RouterEvent) => event instanceof NavigationEnd)
    //).subscribe(() => {
    //this.reportsService.getPurchaseReport(apartmentId).subscribe(result => this.purchaseReport=result, error=>console.error(error));
    //this.reportsService.getIncomeReports(apartmentId, 2018).subscribe(result => this.incomeReport = result, error => console.error(error));
    //});

  }

  onChange(e) {
    this.reportsService.getIncomeReports(this.apartmentId, this.selectedYear).subscribe(result => this.incomeReport = result, error => console.error(error));
  }

}
