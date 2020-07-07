import { Component, OnInit, ViewChild } from '@angular/core';
import { IssuesService } from '../issues.service';
import { IIssue, IMessage } from '../../models';
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
  issues: IIssue[];
  messages: IMessage[];
  displayedColumns: string[] = ['id', 'dateOpen', 'title', 'priority', 'description'];


  ngOnInit() {
    this.issuesService.getOpenIssues().subscribe(result => {
      this.dataSourceIssues.data = result;
      this.issues = result;
      this.dataSourceIssues.sort = this.sort;
    });

    // this.issuesService.getOpenIssuesWithMessages().subscribe(result => {
    //   this.messages = result;
    // });
  }

  checkNewMessages(issue: IIssue) {
    return issue.messages.some(a => !a.isRead);
  }
  countNewMessages(issue: IIssue) {
    return issue.messages.filter(a => !a.isRead).length;
  }

onMessagePanelOpen(issue )
{
    issue.isOpen = true;
    if(issue.messages.length==0)
    {
      issue.showNewTextInput=true;
    }
}
onMessagePanelClose(issue )
{
    issue.isOpen = false;
    if(issue.messages.length==0)
    {
      issue.showNewTextInput=false;
    }
}

  openDiscussion() {
    console.log('sdf');
  }
  printId(id) {
    console.log(id);
  }
}


