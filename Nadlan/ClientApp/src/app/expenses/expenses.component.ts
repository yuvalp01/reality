import { Component, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { ITransaction, IAccount } from '../models';
import { ExpensesService } from '../services/expenses.service';
import { MatDialog, MatSort, MatTableDataSource, MatDialogRef } from '@angular/material';
import { ReportService } from '../services/reports.service';
import { AddExpenseComponent } from '../expenses/expenses-form.component';



@Component({
  templateUrl: './expenses.component.html',
  styles: ['table{width:100%}']
})
export class ExpensesComponent implements OnInit {
  displayedColumnsAssistant: string[] = ['date', 'apartmentId', 'amount', 'comments', 'hours', 'actions'];
  dataSourceExpenses = new MatTableDataSource<ITransaction>();
  dataSourceAssistant = new MatTableDataSource<ITransaction>();
  selectedApartment: any;
  assistantBalance: number;

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(
    private expensesService: ExpensesService,
    private reportsService: ReportService,
    private dialog: MatDialog,
  /*  private dialogRef: MatDialogRef<AddExpenseComponent>*/ ) {
  }
  ngOnInit(): void {
    this.refreshData();
  }
  refreshData() {
    //refresh tables
    this.expensesService.getExpenses().subscribe(result => {
      let assistantAccount = result as ITransaction[];
      this.dataSourceAssistant.data = assistantAccount;
      this.dataSourceAssistant.sort = this.sort;

    }, error => console.error(error));
    //Refresh balance
    //this.reportsService.getAccountBalance(107).subscribe(result => this.assistantBalance = result, error => console.error(error));
    this.reportsService.getExpensesBalance().subscribe(result => this.assistantBalance = result, error => console.error(error));
  }

  //dialogRef: MatDialogRef<AddTransactionComponent>;
  openAddHoursDialog() {
    let dialogRef = this.dialog.open(AddExpenseComponent, {
      height: '600px',
      width: '500px',
      data: { type: 'hours', visibleAccounts: [4, 6, 11] },
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
    dialogRef.afterClosed().subscribe(result => {
      this.refreshData();
    });

  }


  openAddExpensesDialog() {
    // let dialogRef = this.dialog.open(AddTransactionComponent, {

    let dialogRef = this.dialog.open(AddExpenseComponent, {
      height: '600px',
      width: '500px',
      data: { type: 'expenses', visibleAccounts: [4, 6, 8, 11] },
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData();
    });

  }

  openEdit(transactionId) {
    console.log('transactionId: ' + transactionId)

    this.expensesService.getExpense(transactionId).subscribe(result => {
      let _expense: ITransaction = result;
      let _type: string = _expense.hours == 0 ? 'expenses' : 'hours'

      let dialogRef = this.dialog.open(AddExpenseComponent, {
        height: '600px',
        width: '500px',
        data: {
          type: _type,
          expense: _expense,
          visibleAccounts: [4, 6, 8, 11]
        },
      });
      dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
      dialogRef.afterClosed().subscribe(result => {
        this.refreshData();
      });

    }, error => console.log(error));

  }



  ngAfterViewInit(): void {
    this.dataSourceAssistant.sort = this.sort;
    this.dataSourceExpenses.sort = this.sort;
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

  delete(transactionId) {
    console.log(transactionId);
    if (confirm("Are you sure you want to delete?")) {
      this.expensesService.deleteExpense(transactionId).subscribe(
        {
          next: () => this.refreshData(),
          error: err => console.error(err)
        });
    }
  }

  confirm(transactionId) {
    this.expensesService.confirmExpense(transactionId).subscribe(() => {
      this.refreshData();
    });
  }

}
