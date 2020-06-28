import { Component, OnInit, ViewChild } from '@angular/core';
import { IssuesService } from '../issues.service';
import { IIssue } from '../../models';
import { MatTableDataSource, MatSort } from '@angular/material';

@Component({
  selector: 'app-open-issues',
  templateUrl: './open-issues.component.html',
  styleUrls: ['./open-issues.component.css']
})
export class OpenIssuesComponent implements OnInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;  
  constructor(private issuesService: IssuesService) { }

  dataSourceIssues = new MatTableDataSource<IIssue>();

  ngOnInit() {
    this.issuesService.getOpenIssues().subscribe(result => {
      this.dataSourceIssues.data = result;
      debugger
      this.dataSourceIssues.sort = this.sort;
    });
  }
}


