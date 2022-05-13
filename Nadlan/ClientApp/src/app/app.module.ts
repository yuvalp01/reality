import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ApartmentListComponent } from './fetch-data/fetch-apart.component';
import { ApartmentService } from './services/apartment.service';
import { AccountService } from './services/account.service';
import { AccountListComponent } from './fetch-data/fetch-accounts.component';
import { TransactionService } from './services/transaction.service';
import { TransactionListComponent } from './transactions/fetch-transactions.component';
import { TransactionListComponent_ } from './transactions/transaction-list/transaction-list.component';
import { AddApartmentForm } from './forms/add-apartment.component';
import { ReportService } from './services/reports.service';
import { ReportsComponent } from './reports/reports.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MyOwnCustomMaterialModule } from './shared/cusotom-material';
import { MAT_DATE_LOCALE, MatDialogRef, MAT_DIALOG_DATA, DateAdapter, MAT_DATE_FORMATS } from '@angular/material';
import { TransactionsDialogComponent } from './transactions/transactions-dialog.component';
import { RenovationService } from './services/renovation.service';
import { ExcelService } from './services/excel.service';
import { ExpensesComponent } from './expenses/expenses.component';
import { AddExpenseComponent } from './expenses/expenses-form.component';
import { InvestrorsModule } from './investors/investrors.module';
import { IssuesModule } from './issues/issues.module';
import { ExpensesService } from './services/expenses.service';
import { SharedModule } from './shared/shared.module';
import { WelcomepageComponent } from './welcomepage/welcomepage.component';
import { TransactionFormComponent } from './transactions/transaction-form/transaction-form.component';
import { LoginComponent } from './security/login/login.component';
import { HttpInterceptorModule } from './security/http-interceptor';
import { RenovationOverviewComponent } from './renovationNew/renovation-overview/renovation-overview.component';
import { PaymentFormComponent } from './renovationNew/payment-form/payment-form.component';
import { AppRoutingModule } from './app.routing.module';
import { ContractListComponent } from './contracts/contract-list/contract-list.component';
import { ContractFormComponent } from './contracts/contract-form/contract-form.component';
import { ContractPaymentsComponent } from './contracts/contract-payments/contract-payments.component';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { ProductsComponent } from './renovationNew/products/products.component';
import { ProductFormComponent } from './renovationNew/product-form/product-form.component';
import { DriveLinkGenComponent } from './shared/drive-link-gen/drive-link-gen.component';
import { PlanningComponent } from './renovationNew/planning/planning.component';
import { BankAccountService } from './services/bankAaccount.service';
import { InsuranceListComponent } from './insurances/insurance-list/insurance-list.component';
import { InsuranceFormComponent } from './insurances/insurance-form/insurance-form.component';
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ApartmentListComponent,
    AddApartmentForm,
    AccountListComponent,
    TransactionListComponent,
    TransactionListComponent_,
    ReportsComponent,
    TransactionsDialogComponent,
    ExpensesComponent,
    AddExpenseComponent,
    WelcomepageComponent,
    TransactionFormComponent,
    LoginComponent,
    RenovationOverviewComponent,
    PaymentFormComponent,
    ContractListComponent,
    ContractFormComponent,
    ContractPaymentsComponent,
    ProductsComponent,
    ProductFormComponent,
    DriveLinkGenComponent,
    PlanningComponent,
    InsuranceListComponent,
    InsuranceFormComponent,

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MyOwnCustomMaterialModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    InvestrorsModule,
    IssuesModule,
    HttpInterceptorModule
  ],

  providers: [ApartmentService,
    AccountService,
    BankAccountService,
    TransactionService,
    ExpensesService,
    ReportService,
    RenovationService,
    ExcelService,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: MatDialogRef, useValue: {} }],
  bootstrap: [AppComponent],
  entryComponents: [
    TransactionsDialogComponent,
    TransactionFormComponent,
    AddExpenseComponent,
    ContractFormComponent,
    InsuranceFormComponent,
    ProductFormComponent,
    PaymentFormComponent]
})
export class AppModule { }
