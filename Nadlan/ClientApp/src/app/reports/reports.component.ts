import { Component, OnInit, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction, IApartment } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { ApartmentService } from '../services/apartment.service';

@Component({
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  constructor(
    private reportsService: ReportService,
    private apartmentService: ApartmentService,
    private route: ActivatedRoute) {
  }
  apartments: IApartment[];
  selectedApartmentId: number;
  ngOnInit(): void {
    this.selectedApartmentId = this.route.snapshot.params['apartmentId'];
    this.apartmentService.getApartments().subscribe(result => {
      this.apartments = result;
      this.route.paramMap.subscribe(params => {
        this.selectedApartmentId = +params.get("apartmentId");
      });
    }, error => console.error(error));


  //showReport(apartmentId: number) {
  //  this.selectedApartmentId = apartmentId;
  //  //this.route.snapshot.params['apartmentId'] = this.selectedApartmentId;

  //}
}
}
