import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OpenIssuesComponent } from './open-issues/open-issues.component';
import { SharedModule } from '../shared/shared.module';
import { MyOwnCustomMaterialModule } from '../shared/cusotom-material';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [OpenIssuesComponent],
  imports: [
    CommonModule,
    MyOwnCustomMaterialModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'open-issues', component: OpenIssuesComponent },
    ])
  ]
})
export class IssuesModule { }
