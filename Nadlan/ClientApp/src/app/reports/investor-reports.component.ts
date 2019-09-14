import { Component, OnInit } from "@angular/core";
import { IInvestorReportOverview } from "../models";
import { ReportService } from "../services/reports.service";

@Component({
  templateUrl: './investor-reports.component.html',
  styleUrls:['./investor-reports.component.css']
})
export class InvestorReportComponent implements OnInit {

  constructor(private reportService:ReportService) {
  }
  investorReportOverview: IInvestorReportOverview;

  ngOnInit(): void {
    this.reportService.getInvestorReport(101).subscribe(result => this.investorReportOverview = result, error => console.error(error));
  }






}
