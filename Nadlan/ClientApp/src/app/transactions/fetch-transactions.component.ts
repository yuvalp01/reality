import { Component, OnInit, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { ExpensesService } from '../services/expenses.service';

@Component({
  templateUrl: './fetch-transactions.component.html',
  //styles: ['table{width:100%}']
  styleUrls: ['./fetch-transactions.component.css']
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'date', 'apartmentId', 'accountId', 'amount', 'isPurchaseCost', 'comments', 'actions'];
  dataSource = new MatTableDataSource<ITransaction>();

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  selectedApartment: any;
  constructor(
    private transactionService: TransactionService,
    private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.transactionService.getTransactions().subscribe(result => {

      this.dataSource.data = result as ITransaction[];
      this.dataSource.sort = this.sort;
      this.dataSource.filterPredicate = function (data, filter: string): boolean {
        return data.apartmentAddress.toLowerCase().includes(filter);
      };
    }, error => console.error(error));
  }

  refreshData() {
    //refresh tables
    this.transactionService.getTransactions().subscribe(result => {
      this.dataSource.data = result as ITransaction[];
      this.dataSource.sort = this.sort;
      this.dataSource.filterPredicate = function (data, filter: string): boolean {
        return data.apartmentAddress.toLowerCase().includes(filter);
      };
    }, error => console.error(error));
  }

  openForm(_transactionId: number) {
    let dialogRef = this.dialog.open(TransactionFormComponent, {
      height: '600px',
      width: '500px',
      data: { transactionId: _transactionId }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
  }
  delete(transactionId) {
    if (confirm("Are you sure you want to delete?")) {
      this.transactionService.deteleTransaction(transactionId)
        .subscribe({
          next: () => this.onDeleteComplete(),
          error: err => console.error(err)
        });
    }
  }
  onDeleteComplete() {
    this.refreshData();
  }

  confirm(transactionId) {
    this.transactionService.confirmTransaction(transactionId).subscribe(() => {
      this.refreshData();
    });
  }


  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    // this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();

  }

  doFilter(value: string) {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  filter() {
    this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();

  }
  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

}
  //openDialog()
  //{

  //  let dialogRef = this.dialog.open(AddTransactionComponent, {
  //    height: '600px',
  //    width: '500px',
  //    data: {type:'full'}
  //  }).afterClosed().subscribe(item => {

  //    this.transactionService.getTransactions().subscribe(result => {

  //      this.dataSource.data = result as ITransaction[];
  //      this.dataSource = new MatTableDataSource(result);
  //      this.dataSource.sort = this.sort
  //      //this.filter();
  //    }, error => console.error(error));

  //  });
  //}
