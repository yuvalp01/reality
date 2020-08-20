import { Component, OnInit, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { ExpensesService } from '../services/expenses.service';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { AddExpenseComponent } from '../expenses/expenses-form.component';
import { TransactionService } from '../services/transaction.service';
import { ExcelService } from '../services/excel.service';




@Component({
  templateUrl: './expenses.component.html',
  styles: ['table{width:100%}']
})
export class ExpensesComponent implements OnInit {
  displayedColumnsAssistant: string[] = ['date', 'isPurchaseCost', 'apartmentId', 'amount', 'comments', 'hours', 'actions'];
  dataSourceAssistant = new MatTableDataSource<ITransaction>();
  selectedApartment: any;
  assistantBalance: number = 0;
  visibleAccountsHours: number[] = [4, 6, 11, 16,18, 200];
  visibleAccountsExpenses: number[] = [1, 4, 6, 17,18, 11, 8, 16, 198, 200, 201];
  //role: number;
  monthsBack:number = 3;
  //showFullList:boolean = false;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(
    private expensesService: ExpensesService,
    private transactionService: TransactionService,
    private excelService: ExcelService,
    private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.refreshData();
  }
  loadAllList()
  {
    this.monthsBack = 0;
    this.refreshData();
  }
  refreshData() {
    //refresh tables
    this.expensesService.getExpenses(this.monthsBack).subscribe(result => {
      let assistantAccount = result as ITransaction[];
      this.dataSourceAssistant.data = assistantAccount;
      this.dataSourceAssistant.sort = this.sort;

    }, error => console.error(error));
    //Refresh balance
    this.expensesService.getExpensesBalance().subscribe(
      result => {
        this.assistantBalance = result
      },
      error => {
        console.error(error)
      });
  }

  applyFilter(filterValue: string) {
    this.dataSourceAssistant.filter = filterValue.trim().toLocaleLowerCase();

  }

  openAddMode(actionType: string) {
    let _visibleAccounts;
    if (actionType == 'expenses') {
      _visibleAccounts = this.visibleAccountsExpenses;
    }
    else {
      _visibleAccounts = this.visibleAccountsHours;
    }

    let dialogRef = this.dialog.open(AddExpenseComponent, {
      height: '600px',
      width: '500px',
      data: { type: actionType, visibleAccounts: _visibleAccounts },
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData();
    });

  }


  openEdit(transactionId) {
    this.expensesService.getExpense(transactionId).subscribe(result => {
      let _expense: ITransaction = result;
      let _type;
      let _visibleAccounts
      if (_expense.hours == 0) {
        _type = 'expenses';
        _visibleAccounts = this.visibleAccountsExpenses;
      }
      else {
        _type = 'hours';
        _visibleAccounts = this.visibleAccountsHours;
      }

      //let _type: string = _expense.hours == 0 ? 'expenses' : 'hours'

      let dialogRef = this.dialog.open(AddExpenseComponent, {
        height: '600px',
        width: '500px',
        data: {
          type: _type,
          expense: _expense,
          visibleAccounts: _visibleAccounts
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
    //this.dataSourceExpenses.sort = this.sort;
  }

  //doFilter(value: string) {
  //  this.dataSourceAssistant.filter = value.trim().toLocaleLowerCase();
  //}

  //filter() {
  //  this.dataSourceAssistant.filter = this.selectedApartment.trim().toLocaleLowerCase();

  //}

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
    this.transactionService.confirmTransaction(transactionId).subscribe(() => {
      this.refreshData();
    });
  }

  exportAsXLSX(): void {

    //this.dataSourceAssistant.data.forEach(a => delete a.stakeholderId);
   var xxxx= this.filterByString(this.dataSourceAssistant.data,'bouboulinas')



    this.excelService.exportAsExcelFile(xxxx, 'Transactions');
   // this.excelService.exportAsExcelFile(this.dataSourceAssistant.data, 'Transactions');
  }

  filterByString(data, s) {
    return data.filter(e => e.apartmentId.includes(s) || e.comments.includes(s));
      //.sort((a, b) => a.id.includes(s) && !b.id.includes(s) ? -1 : b.id.includes(s) && !a.id.includes(s) ? 1 : 0);
  }
}

