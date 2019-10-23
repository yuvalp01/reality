import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PersonalAdminComponent } from './personal-admin/personal-admin.component';
import { PersonalTransComponent } from './personal-trans/personal-trans.component';
import { PersonalTransFormComponent } from './personal-trans-form/personal-trans-form.component';
import { MyOwnCustomMaterialModule } from '.././shared/cusotom-material';
import { ReactiveFormsModule } from '@angular/forms';
import { InvestorReportComponent } from './investor-reports/investor-reports.component';
import { PersonalTransDialogComponent } from './personal-trans-dialog/personal-trans-dialog.component';


@NgModule({
  declarations: [
    PersonalAdminComponent,
    PersonalTransComponent,
    PersonalTransFormComponent,
    InvestorReportComponent,
    PersonalTransDialogComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MyOwnCustomMaterialModule,
    RouterModule.forChild([
      { path: 'personal-trans', component: PersonalAdminComponent, pathMatch: 'full' },
      { path: 'personal-admin/:stakeholderId', component: PersonalAdminComponent },
      { path: 'investor-reports/:stakeholderId', component: InvestorReportComponent}

    ])
  ],
  entryComponents: [PersonalTransFormComponent, PersonalTransDialogComponent]
})
export class InvestrorsModule { }
