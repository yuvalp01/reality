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
import { ReportService } from './services/reports.service';
import { ApartmentReportsComponent } from './reports/apartment-reports.component';
import { ReportsComponent } from './reports/reports.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MyOwnCustomMaterialModule } from './shared/cusotom-material';
import { MAT_DATE_LOCALE, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MinusSignToParens } from './shared/minusSignToParens';
import { TransactionsDialogComponent } from './transactions/transactions-dialog.component';
import { RenovationComponent } from './renovation/renovation.component';
import { RenovationService } from './services/renovation.service';
import { ExcelService } from './services/excel.service';
import { RenovationListComponent } from './renovation/renovation-list.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { TransactionsTableComponent } from './transactions/transactions-table.component';
import { AddExpenseComponent } from './expenses/expenses-form.component';
import { InvestrorsModule } from './investors/investrors.module';
import { ExpensesService } from './services/expenses.service';
import { SharedModule } from './shared/shared.module';
import { WelcomepageComponent } from './welcomepage/welcomepage.component';
import { TransactionFormComponent } from './transactions/transaction-form/transaction-form.component';
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
    ReportsComponent,
    //ApartmentReportsComponent,
    //MinusSignToParens,
    TransactionsDialogComponent,
    RenovationComponent,
    RenovationListComponent,
    ExpensesComponent,
    TransactionsTableComponent,
    AddExpenseComponent,
    WelcomepageComponent,
    TransactionFormComponent,

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MyOwnCustomMaterialModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: WelcomepageComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'add-apartment', component: AddApartmentForm },
      { path: 'fetch-accounts', component: AccountListComponent },
      { path: 'fetch-transactions', component: TransactionListComponent },
      { path: 'reports/:apartmentId', component: ReportsComponent },
      { path: 'reports/:apartmentId/:status', component: ReportsComponent },


      //{ path: 'reports/:apartmentId', component: ApartmentReportsComponent },
      { path: 'renovation', component: RenovationComponent },
      { path: 'renovation-list', component: RenovationListComponent },
      { path: 'expenses', component: ExpensesComponent },

    ]),
    SharedModule,
    InvestrorsModule,
    //ApartmentReportsComponent
  ],

  providers: [ApartmentService,
    AccountService,
    TransactionService,
    ExpensesService,
    ReportService,
    RenovationService,
    ExcelService,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: MatDialogRef, useValue: {} }],
  bootstrap: [AppComponent],
  entryComponents: [TransactionsDialogComponent, TransactionFormComponent, AddExpenseComponent]
})
export class AppModule { }
