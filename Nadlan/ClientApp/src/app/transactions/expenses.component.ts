import { Component, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { ITransaction, IAccount } from '../models';
import { TransactionService } from '../services/transaction.service';
import { AddTransactionComponent } from '../transactions/add-transaction.component';
import { MatDialog, MatSort, MatTableDataSource, MatDialogRef } from '@angular/material';
import { ReportService } from '../services/reports.service';
import { RouteReuseStrategy } from '@angular/router';


@Component({
  templateUrl: './expenses.component.html',
  styles: ['table{width:100%}']
})
export class ExpensesComponent implements OnInit {
  displayedColumnsExpenses: string[] = ['date', 'apartmentId', 'accountId', 'amount', 'comments'];
  displayedColumnsAssistant: string[] = ['date', 'apartmentId', 'amount', 'comments','actions'];
  dataSourceExpenses = new MatTableDataSource<ITransaction>();
  dataSourceAssistant = new MatTableDataSource<ITransaction>();
  //expensesAccounts: IAccount[]
  selectedApartment: any;
  assistantBalance: number;

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(private transactionService: TransactionService, private reportsService:ReportService, private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.refreshData();
    //this.expensesAccounts = accou
  }


  refreshData() {
    //refresh tables
    this.transactionService.getTransactions().subscribe(result => {
      let transactions = result as ITransaction[];

      let assistantAccount = transactions.filter(a => a.accountId == 107);
      this.dataSourceAssistant.data = assistantAccount;
      this.dataSourceAssistant.sort = this.sort;

      let expenses = transactions.filter(a => a => a.id == 4 || a.id == 6 || a.id == 11);
      this.dataSourceExpenses.data = expenses;
      this.dataSourceExpenses.sort = this.sort;

    }, error => console.error(error));
    //Refresh balance
    this.reportsService.getAccountBalance(107).subscribe(result => this.assistantBalance = result, error => console.error(error));
  }

  dialogRef: MatDialogRef<AddTransactionComponent>;
  openAddHoursDialog() {
    this.dialogRef = this.dialog.open(AddTransactionComponent, {
      height: '600px',
      width: '500px',
      //data: 'hours'
      data: { type: 'hours', visibleAccounts: [4, 6, 11] },
    });
    this.dialogRef.afterClosed().subscribe(result => {
      this.refreshData();
      //this.reportsService.getAccountBalance(107).subscribe(result => this.assistantBalance = result, error => console.error(error));
    });


    //const sub = this.dialogRef.componentInstance.balanceChanged.emit("ddd");
  }
    

  openAddExpensesDialog() {
    let dialogRef = this.dialog.open(AddTransactionComponent, {
      height: '600px',
      width: '500px',
      //data: 'expenses'
      data: { type: 'expenses', visibleAccounts:[4,6,8,11] },
    });
    dialogRef.afterClosed().subscribe(result => {
    //  console.log(`Dialog result: ${result}`); 
      this.refreshData();
      //this.reportsService.getAccountBalance(107).subscribe(result => this.assistantBalance = result, error => console.error(error));
      });
    //this.dialogRef.componentInstance.balanceChanged.emit((null) => {
    //  this.refreshData();

    //});
  }

  openEdit(transactionId) {
    console.log('transactionId: ' + transactionId)
  }



  ngAfterViewInit(): void {
    this.dataSourceAssistant.sort = this.sort;
    this.dataSourceExpenses.sort = this.sort;


    // this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();

  }

  doFilter(value: string) {
    this.dataSourceAssistant.filter = value.trim().toLocaleLowerCase();
    this.dataSourceExpenses.filter = value.trim().toLocaleLowerCase();
  }

  filter() {
    this.dataSourceAssistant.filter = this.selectedApartment.trim().toLocaleLowerCase();
    this.dataSourceExpenses.filter = this.selectedApartment.trim().toLocaleLowerCase();

  }

  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }




}
