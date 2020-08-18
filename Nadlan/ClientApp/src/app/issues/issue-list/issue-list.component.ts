import { Component, OnInit } from '@angular/core';
import { IssuesService } from '../issues.service';
import { IIssue } from 'src/app/models';
import { MatDialog, MatExpansionPanel, MatSnackBar } from '@angular/material';
import { IssueFormComponent } from '../issue-form/issue-form.component';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { MessagesService } from '../meassages.service';
import { SecurityService } from 'src/app/security/security.service';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
})
export class IssueListComponent implements OnInit {

  constructor(private issueService: IssuesService,
    private messagesService: MessagesService,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog) { }
  //issues: IIssue[];
  closedIssues: IIssue[];
  openIssues: IIssue[];
  currentUser: string = 'unknown';

  ngOnInit() {
    if (this.securityService.securityObject.userName != '') {
      this.currentUser = this.securityService.securityObject.userName;
    }
    this.loadList();
  }

  openIssueForm(item: IIssue) {
    let dialogLocal = this.dialog.open(IssueFormComponent, {
      height: 'auto',
      width: 'auto',
      data: item
    });
    dialogLocal.componentInstance.refreshEmitter.subscribe(() => this.loadList())
  }
  loadList() {
    this.issueService.getOpenIssues().subscribe({
      next: result => {
        this.closedIssues = result.filter(a => a.dateClose != null);
        this.openIssues = result.filter(a => a.dateClose == null);
        this.checkNewMessages();

      },
      error: err => console.error(err)
    });
  }
  checkNewMessages() {
    this.openIssues.forEach(issue => {
      if(issue.messages.length==0)
      {
        issue['hasUnread'] = false;
      }
      else
      { 
   // let otherUserMessages = issue.messages.filter(a => a.userName.toLowerCase() != this.currentUser);
      let unread = issue.messages.filter(a => a.userName.toLowerCase() != this.currentUser && !a.isRead);
    //  let unread = issue.messages.some(a => !a.isRead && a.userName != this.currentUser);
      if (unread.length>0) issue['hasUnread'] = true;
      else issue['hasUnread'] = false;
      }
    });
  }


delete (id: number) {
  if (confirm("Are you sure you want to delete?")) {
    this.issueService.delete(id).subscribe({
      next: () => {
        this.snackBar.open(`Issue id ${id}`, 'Deleted', { duration: 2000 });
        this.loadList();
      },
      error: (err) => console.error(err)
    })
  }
}



openMessages(id: number) {

  let dialogLocal = this.dialog.open(MessageBoxComponent, {
    height: 'auto',
    width: 'auto',
    data: { tableName: 'issues', id: id }
  });
  dialogLocal.afterClosed().subscribe(()=>this.loadList())
  // dialogLocal.componentInstance.refreshEmitter.subscribe(() => this.loadList())
}



  // expandPanel(matExpansionPanel: MatExpansionPanel, event: Event): void {
  //   event.stopPropagation(); // Preventing event bubbling

  //   if (!this._isExpansionIndicator(event.target)) {
  //     matExpansionPanel.close(); // Here's the magic
  //   }
  // }

  // private _isExpansionIndicator(target: any): boolean {
  //   const expansionIndicatorClass = 'mat-expansion-indicator';
  //   return (target.classList && target.classList.contains(expansionIndicatorClass) );
  // }
}
