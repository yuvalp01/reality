import { Component, OnInit } from '@angular/core';
import { ReportService } from '../services/reports.service';

@Component({
  selector: 'app-personal-trans',
  templateUrl: './personal-trans.component.html',
  styleUrls: ['./personal-trans.component.css']
})
export class PersonalTransComponent implements OnInit {

  constructor(private reportService: ReportService) { }

  balance: number = 0;

  ngOnInit() {
    //this.reportService.getPersonalBalance(1).subscribe(result=>this.balance=result,error=>console.error(error));
  }
}
