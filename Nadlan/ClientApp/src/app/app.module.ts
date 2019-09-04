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
import { TransactionListComponent } from './fetch-data/fetch-transactions.component';
import { AddApartmentForm } from './forms/add-apartment.component';
import { AddTransactionComponent} from './forms/add-transaction.component';
import { AddAccoutComponent } from './forms/add-account.component';
import { ReportService } from './services/reports.service';
import { ReportsComponent } from './fetch-data/reports.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MyOwnCustomMaterialModule } from './shared/cusotom-material';
import { MAT_DATE_LOCALE } from '@angular/material';
import { MinusSignToParens } from './shared/minusSignToParens';
import { TransactionsDialogComponent } from './fetch-data/transactions-dialog.component';
import { RenovationComponent } from './renovation/renovation.component';
import { RenovationService } from './services/renovation.service';
import { ExcelService } from './services/excel.service';
import { RenovationListComponent } from './renovation/renovation-list.component';
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
    MinusSignToParens,
    TransactionsDialogComponent,
    RenovationComponent,
    RenovationListComponent
    
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
      { path: 'fetch-apartments', component: ApartmentListComponent },
      { path: 'add-apartment', component: AddApartmentForm },
      { path: 'fetch-accounts', component: AccountListComponent },
      { path: 'fetch-transactions', component: TransactionListComponent },
      { path: 'add-transaction', component: AddTransactionComponent },
      { path: 'add-account', component: AddAccoutComponent },
      { path: 'reports', component: ReportsComponent },
      { path: 'reports/:apartmentId', component: ReportsComponent },
      { path: 'renovation', component: RenovationComponent },
      { path: 'renovation-list', component: RenovationListComponent },
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
  entryComponents: [TransactionsDialogComponent]
})
export class AppModule { }
