import { Component, OnInit, SimpleChanges, ViewEncapsulation, Output } from "@angular/core";
import { IInvestorReportOverview, IStakeholder, IPersonalTransaction } from "../../models";
import { PersonalTransService } from "../personal-trans.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource, MatDialog } from "@angular/material";
import { PersonalTransDialogComponent } from "../personal-trans-dialog/personal-trans-dialog.component";
import { ApartmentReportsComponent } from "src/app/reports/apartment-reports.component";

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
  portfolioColumns = ['apartment', 'purchaseDate', 'investment', 'ownership'];//'minimalProfitUpToDate', 'distributed'
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
  showTrans(transactionType) {
    this.personalTransService.getPesonalTransByType(this.stakeholderId, transactionType).subscribe(
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

}
