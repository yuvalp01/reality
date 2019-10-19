import { Component, OnInit } from '@angular/core';
import { PersonalTransService } from 'src/app/services/personal-trans.service';
import { ReportService } from 'src/app/services/reports.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-personal-admin',
  templateUrl: './personal-admin.component.html',
  styleUrls: ['./personal-admin.component.css']
})
export class PersonalAdminComponent implements OnInit {

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute) { }
  balance: number = 0;
  stakeholderId: number;
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.stakeholderId = +params.get("stakeholderId");
      //this.reportService.getPersonalBalance(this.stakeholderId).subscribe(result => this.balance = result, error => console.error(error));
    });
  }
}
