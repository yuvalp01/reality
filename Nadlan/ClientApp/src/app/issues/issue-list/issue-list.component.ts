import { Component, OnInit } from '@angular/core';
import { IssuesService } from '../issues.service';
import { IIssue } from 'src/app/models';
import { MatDialog, MatExpansionPanel, MatSnackBar } from '@angular/material';
import { IssueFormComponent } from '../issue-form/issue-form.component';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
})
export class IssueListComponent implements OnInit {

  constructor(private issueService: IssuesService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog) { }
  //issues: IIssue[];
  closedIssues:IIssue[];
  openIssues:IIssue[];

  ngOnInit() {
    this.loadList();
  }

  openIssueForm(item:IIssue) {
    let dialogLocal = this.dialog.open(IssueFormComponent, {
      height: 'auto',
      width: 'auto',
      data: item
    });
    dialogLocal.componentInstance.refreshEmitter.subscribe(() => this.loadList())
  }
  loadList() {
    this.issueService.getOpenIssues().subscribe({
      next: result => 
      {
        this.closedIssues = result.filter(a=>a.dateClose!=null);
        this.openIssues = result.filter(a=>a.dateClose==null);

      },
      error: err => console.error(err)
    });
  }
  delete(id: number) {
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
    console.log(id);
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
