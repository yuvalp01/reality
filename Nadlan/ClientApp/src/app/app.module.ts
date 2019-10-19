import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ApartmentListComponent } from './fetch-data/fetch-apart.component';
import { ApartmentService } from './services/apartment.service';
import { AccountService } from './services/account.service';
import { AccountListComponent } from './fetch-data/fetch-accounts.component';
import { TransactionService } from './services/transaction.service';
import { TransactionListComponent } from './transactions/fetch-transactions.component';
import { AddApartmentForm } from './forms/add-apartment.component';
import { AddTransactionComponent } from './transactions/add-transaction.component';
import { AddAccoutComponent } from './forms/add-account.component';
import { ReportService } from './services/reports.service';
import { ApartmentReportsComponent } from './reports/apartment-reports.component';
import { ReportsComponent } from './reports/reports.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MyOwnCustomMaterialModule } from './shared/cusotom-material';
import { MAT_DATE_LOCALE } from '@angular/material';
import { MinusSignToParens } from './shared/minusSignToParens';
import { TransactionsDialogComponent } from './transactions/transactions-dialog.component';
import { RenovationComponent } from './renovation/renovation.component';
import { RenovationService } from './services/renovation.service';
import { ExcelService } from './services/excel.service';
import { RenovationListComponent } from './renovation/renovation-list.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { TransactionsTableComponent } from './transactions/transactions-table.component';
import { InvestorReportComponent } from './reports/investor-reports.component';
import { AddExpenseComponent } from './expenses/expenses-form.component';
import { PersonalTransComponent } from './investors/personal-trans/personal-trans.component';
import { PersonalTransFormComponent } from './investors/personal-trans-form/personal-trans-form.component';
import { PersonalAdminComponent } from './investors/personal-admin/personal-admin.component';
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    ApartmentListComponent,
    AddApartmentForm,
    AccountListComponent,
    TransactionListComponent,
    AddTransactionComponent,
    AddAccoutComponent,
    ReportsComponent,
    ApartmentReportsComponent,
    MinusSignToParens,
    TransactionsDialogComponent,
    RenovationComponent,
    RenovationListComponent,
    ExpensesComponent,
    TransactionsTableComponent,
    InvestorReportComponent,
    AddExpenseComponent,
    PersonalTransComponent,
    PersonalTransFormComponent,
    PersonalAdminComponent
    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MyOwnCustomMaterialModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: TransactionListComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      //{ path: 'fetch-apartments', component: ApartmentListComponent },
      { path: 'add-apartment', component: AddApartmentForm },
      { path: 'fetch-accounts', component: AccountListComponent },
      { path: 'fetch-transactions', component: TransactionListComponent },
      //{ path: 'add-transaction', component: AddTransactionComponent },
      { path: 'add-account', component: AddAccoutComponent },
      //{ path: 'reports', component: InvestorReportComponent },
      { path: 'reports/:apartmentId', component: ReportsComponent },
      //{ path: 'reports/:apartmentId', component: ReportsComponent },
      { path: 'investor-reports', component: InvestorReportComponent },

      
      //{ path: 'reports/:apartmentId', component: ApartmentReportsComponent },
      { path: 'renovation', component: RenovationComponent },
      { path: 'renovation-list', component: RenovationListComponent },
      { path: 'expenses', component: ExpensesComponent },
      { path: 'personal-trans', component: PersonalTransComponent },
      { path: 'personal-admin', component: PersonalAdminComponent },
      { path: 'personal-admin/:stakeholderId', component: PersonalAdminComponent },


      
      
      //{ path: 'reports/:apartmentId', component: TransactionsDialogComponent },

      //{
      //  path: 'reports', component: ReportsComponent, children[
      //    {
      //      path: '',
      //      component:
      //    }
      //              {
      //      path: '',
      //      component:
      //    }
      //  ]
      //}
    ])
  ],
  providers: [ApartmentService, AccountService, TransactionService, ReportService, RenovationService, ExcelService,{provide: MAT_DATE_LOCALE, useValue: 'en-GB'}],
  bootstrap: [AppComponent],
  entryComponents: [TransactionsDialogComponent, AddTransactionComponent, AddExpenseComponent, PersonalTransFormComponent]
})
export class AppModule { }
