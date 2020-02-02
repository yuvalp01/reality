import { Component, OnInit, SimpleChanges, Output } from "@angular/core";
import { IInvestorReportOverview, IStakeholder, IPersonalTransaction } from "../../models";
import { PersonalTransService } from "../personal-trans.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource, MatDialog } from "@angular/material";
import { PersonalTransDialogComponent } from "../personal-trans-dialog/personal-trans-dialog.component";
import { ApartmentReportsComponent } from "src/app/reports/apartment-reports.component";
import { debug } from "util";
import  * as Global  from "src/app/shared/globalVars";

@Component({
  selector: 'investor-reports',
  templateUrl: './investor-reports.component.html',
  styleUrls: ['./investor-reports.component.css'],
  //styles: [`.mat-tab-header {
  //background - color: red!important;
  //}`]
})
export class InvestorReportComponent implements OnInit {

  constructor(
    private personalTransService: PersonalTransService,
    private route: ActivatedRoute,
    private dialog: MatDialog) { }
  @Output() stakeholderId: number;
  investorReportOverview: IInvestorReportOverview;
  stakeholders: IStakeholder[];
  portfolioColumns = ['apartment', 'purchaseDate', 'investment','distributed', 'ownership'];//'minimalProfitUpToDate', 'distributed'
  selectedTab: number;
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.stakeholderId = +params.get("stakeholderId");
      this.refreshData(this.stakeholderId);
    });

  }

  ngOnChanges(changes: SimpleChanges): void {
    this.stakeholderId = changes.stakeholderId.currentValue;
    this.refreshData(this.stakeholderId);
  }

  refreshData(stakeholderId: number) {
    this.personalTransService.getInvestorReport(stakeholderId)
      .subscribe(result => this.investorReportOverview = result, error => console.error(error));
    this.loadAllStakeholders();
  }


  loadAllStakeholders() {
    this.personalTransService.getStakeholders().subscribe({
      next: (result) => this.stakeholders = result,
      error: (error) => console.error(error)
    })
  }

  dataSource = new MatTableDataSource<IPersonalTransaction>();
  showDistributions() {
    this.personalTransService.getAllDistributions(this.stakeholderId).subscribe(
      result => {
        //this.transactions = result;
        this.dataSource.data = result as IPersonalTransaction[];
        const dialogRef = this.dialog.open(PersonalTransDialogComponent, {
          height: 'auto',
          width: 'auto',
          data: { transactions: this.dataSource.data, columns: ['date', 'amount', 'comments', 'apartment'] }
        });
      }
      , error => console.error(error));

  }
  showApartmentReport(apartmentId) {

    const dialogRef = this.dialog.open(ApartmentReportsComponent, {
      height: 'auto',
      width: 'auto',
      data: { apartmentId: apartmentId }
    });
  }

  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

  //changeTab(tabIndex) {
  //  debugger
  //  this.selectedTab = tabIndex;
  //}

}
