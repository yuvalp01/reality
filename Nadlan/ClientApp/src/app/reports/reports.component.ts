import { Component, OnInit, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';

@Component({
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
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
    this.selectedApartmentId = apartmentId;
    //this.route.snapshot.params['apartmentId'] = this.selectedApartmentId;

  }
}
