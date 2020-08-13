import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CounterComponent } from './counter/counter.component';
import { AccountListComponent } from './fetch-data/fetch-accounts.component';
import { TransactionListComponent } from './transactions/fetch-transactions.component';
import { AddApartmentForm } from './forms/add-apartment.component';
import { ReportsComponent } from './reports/reports.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { LoginComponent } from './security/login/login.component';
import { AuthGuard } from './security/auth.guard';
import { RenovationOverviewComponent } from './renovationNew/renovation-overview/renovation-overview.component';

@NgModule({
  declarations: [

  ],
  imports: [
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      // { path: 'counter', component: CounterComponent },
      { path: 'add-apartment', component: AddApartmentForm },
      { path: 'fetch-accounts', component: AccountListComponent },
      {
        path: 'fetch-transactions',
        component: TransactionListComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['admin'] }
      },
      {
        path: 'reports/:apartmentId',
        component: ReportsComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['investor', 'admin'] }
      },
      { path: 'reports/:apartmentId/:status', component: ReportsComponent },
      {
        path: 'expenses', component: ExpensesComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['stella', 'admin'] }
      },
      {
        path: 'renovation-overview/:projectId', component: RenovationOverviewComponent,
        canActivate: [AuthGuard],
        data: { claimType: ['stella', 'admin'] }
      },
      { path: 'login', component: LoginComponent },

    ])
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
