import { Component, OnInit } from "@angular/core";
import { IInvestorReportOverview } from "../../models";
import { PersonalTransService } from "../personal-trans.service";

@Component({
  templateUrl: './investor-reports.component.html',
  styleUrls:['./investor-reports.component.css']
})
export class InvestorReportComponent implements OnInit {

  constructor(private personalTransService: PersonalTransService) {
  }
  investorReportOverview: IInvestorReportOverview;

  ngOnInit(): void {
    this.personalTransService.getInvestorReport(101)
      .subscribe(result => this.investorReportOverview = result, error => console.error(error));
  }






}
