import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MyOwnCustomMaterialModule } from '../shared/cusotom-material';
import { RouterModule } from '@angular/router';
import { IssueListComponent } from './issue-list/issue-list.component';
import { IssueFormComponent } from './issue-form/issue-form.component';
import { AuthGuard } from '../security/auth.guard';
import { AppRoutingModule } from '../app.routing.module';



@NgModule({
  declarations: [IssueListComponent, IssueFormComponent],
  imports: [
    CommonModule,
    MyOwnCustomMaterialModule,
    SharedModule,
    // AppRoutingModule,
    RouterModule.forChild([
      {
        path: 'issue-list', 
        component: IssueListComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['stella', 'admin'] }

      },
      // { path: 'open-issues', component: OpenIssuesComponent },
    ])
  ],
  entryComponents: [IssueFormComponent]
})
export class IssuesModule { }
