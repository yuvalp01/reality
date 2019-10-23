import { Component, OnInit, SimpleChanges } from "@angular/core";
import { IInvestorReportOverview, IStakeholder } from "../../models";
import { PersonalTransService } from "../personal-trans.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  templateUrl: './investor-reports.component.html',
  styleUrls: ['./investor-reports.component.css']
})
export class InvestorReportComponent implements OnInit {

  constructor(
    private personalTransService: PersonalTransService,
    private route: ActivatedRoute) { }
  investorReportOverview: IInvestorReportOverview;
  stakeholders: IStakeholder[];
  stakeholderId: number;

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



}
