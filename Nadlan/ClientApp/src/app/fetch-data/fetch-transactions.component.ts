import { Component, OnInit, Output, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { resource } from 'selenium-webdriver/http';
import { AddTransactionComponent } from '../forms/add-transaction.component';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { subscribeOn } from 'rxjs/operators';

@Component({
  templateUrl: './fetch-transactions.component.html',
  styles: ['table{width:100%}']
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'date', 'apartmentId','accountId' , 'amount', 'isPurchaseCost', 'comments'];
  //transactions: ITransaction[];
  dataSource = new MatTableDataSource<ITransaction>();

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  selectedApartment: any;
  constructor(private transactionService: TransactionService, private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.transactionService.getTransactions().subscribe(result => {

      this.dataSource.data = result as ITransaction[];
      //this.transactions = result;
      //this.dataSource = new MatTableDataSource(result);
      this.dataSource.sort = this.sort;
      //this.dataSource.filter = this.selectedApartment;




      this.dataSource.filterPredicate = function (data, filter: string): boolean {
        return data.apartmentAddress.toLowerCase().includes(filter);
      };


      }, error => console.error(error));
}
//dialogRef:any;
openDialog()
{

  let dialogRef = this.dialog.open(AddTransactionComponent, {
    height: '600px',
    width: '500px',
  }).afterClosed().subscribe(item => {

    this.transactionService.getTransactions().subscribe(result => {

      this.dataSource.data = result as ITransaction[];
      //this.transactions = result;
      this.dataSource = new MatTableDataSource(result);
      this.dataSource.sort = this.sort
      //this.filter();
    }, error => console.error(error));

    //this.transactionService.getTransactions().subscribe(result=>this.transactions=result, error=>console.error(error));
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
