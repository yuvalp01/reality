import { Component, OnInit } from '@angular/core';
import { PersonalTransService } from '../personal-trans.service';
import { ActivatedRoute } from '@angular/router';
import { IStakeholder } from '../../models';

@Component({
  selector: 'app-personal-admin',
  templateUrl: './personal-admin.component.html',
  styleUrls: ['./personal-admin.component.css']
})
export class PersonalAdminComponent implements OnInit {

  constructor(
    private personalTransService: PersonalTransService,
    private route: ActivatedRoute) { }
  balance: number = 0;
  stakeholderId: number;
  stakeholders: IStakeholder[];
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.stakeholderId = +params.get("stakeholderId");
      //this.reportService.getPersonalBalance(this.stakeholderId).subscribe(result => this.balance = result, error => console.error(error));
    });
    this.loadAllStakeholders();
  }

  loadAllStakeholders() {
    this.personalTransService.getStakeholders().subscribe({
      next: (result) => this.stakeholders = result,
      error: (error)=> console.error(error)
    })
  }
}
