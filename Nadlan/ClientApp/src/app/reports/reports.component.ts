import { Component, OnInit, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { IIncomeReport, IPurchaseReport, ISummaryReport, ITransaction, IApartment } from '../models';
import { ReportService } from '../services/reports.service';
import { ActivatedRoute, RouterEvent, NavigationEnd } from '@angular/router';
import { ApartmentService } from '../services/apartment.service';
import { debug } from 'util';

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
  status: string;
  ownershipType: number;
  ngOnInit(): void {
    //this.selectedApartmentId = this.route.snapshot.params['apartmentId'];

    this.route.paramMap.subscribe(params => {
      this.status = params.get('status');
      this.ownershipType = +params.get('ownershipType');
      this.selectedApartmentId = +params.get('apartmentId');
      if (this.ownershipType) {
        this.apartmentService.getApartmentsByOwnership(this.ownershipType).subscribe(result => {
          this.apartments = result;
        }, error => console.error(error));
      }
      //TODO: remove, not relevant anymore
      else {
        this.apartmentService.getApartments().subscribe(result => {
          this.apartments = result;
          if (this.status == 'rented') {
            this.apartments = result.filter(a => a.status == 100 && a.id > 0);
          }
          else {
            this.apartments = result.filter(a => a.status != 100 && a.id > 0);
          }

          //this.route.paramMap.subscribe(params => {
          //  this.selectedApartmentId = +params.get("apartmentId");
          //});
        }, error => console.error(error));
      }
    });

  }
}
