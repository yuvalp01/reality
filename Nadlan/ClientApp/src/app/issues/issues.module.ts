import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OpenIssuesComponent } from './open-issues/open-issues.component';
import { SharedModule } from '../shared/shared.module';
import { MyOwnCustomMaterialModule } from '../shared/cusotom-material';
import { RouterModule } from '@angular/router';
import { IssueListComponent } from './issue-list/issue-list.component';
import { IssueFormComponent } from './issue-form/issue-form.component';



@NgModule({
  declarations: [IssueListComponent,OpenIssuesComponent, IssueFormComponent],
  imports: [
    CommonModule,
    MyOwnCustomMaterialModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'issue-list', component: IssueListComponent },
      { path: 'open-issues', component: OpenIssuesComponent },
    ])
  ],
  entryComponents:[IssueFormComponent]
})
export class IssuesModule { }
