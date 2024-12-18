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
import { SharedModule } from '../shared/shared.module';
import { ApartmentReportsComponent } from '../reports/apartment-reports.component';
import { AuthGuard } from '../security/auth.guard';

//import { ApartmentReportsComponent } from '../reports/apartment-reports.component';


@NgModule({
  declarations: [
    PersonalAdminComponent,
    PersonalTransComponent,
    PersonalTransFormComponent,
    InvestorReportComponent,
    PersonalTransDialogComponent,
    //ApartmentReportsComponent
    ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MyOwnCustomMaterialModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'personal-trans', component: PersonalAdminComponent, pathMatch: 'full' },
      { path: 'personal-admin/:stakeholderId', component: PersonalAdminComponent },
      {
        path: 'investor-reports/:stakeholderId/:ownershipType',
        component: InvestorReportComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['investor', 'admin','owner'] },
        children: [
          {
            //path: '', redirectTo: '', pathMatch: 'full',
            //path: 'investor-reports/:stakeholderId/:apartmentId', component: ApartmentReportsComponent,
            path: 'investor-reports/info', component: ApartmentReportsComponent,

          },
          {
            path: ':apartmentId', component: ApartmentReportsComponent,

          }
        ]
      }

    ])
  ],
  //exports: [ApartmentReportsComponent],
  entryComponents: [PersonalTransFormComponent, PersonalTransDialogComponent]
})
export class InvestrorsModule { }
