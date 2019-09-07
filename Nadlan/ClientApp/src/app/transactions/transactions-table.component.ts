import { Component, OnInit, Output, ViewChild, Input } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { AddTransactionComponent } from '../transactions/add-transaction.component';
import { MatDialog, MatSort, MatTableDataSource, MatDialogRef } from '@angular/material';
import { debounce } from 'rxjs/operators';


@Component({
  templateUrl: './transactions-table.component.html',
  styles: ['table{width:100%}'],
  selector: 'transactions-table',
})
export class TransactionsTableComponent implements OnInit {
 // displayedColumns: string[] = ['id', 'date', 'apartmentId','accountId' , 'amount', 'isPurchaseCost', 'comments'];
  @Input() dataList: ITransaction[];

  @Input() displayedColumns: string[];
  @Input() dataSourceObj = new MatTableDataSource<ITransaction>();
  @Input() sort: MatSort;
  //@Input() data: ITransaction[];

  //@ViewChild(MatSort, { static: true }) sort: MatSort;
  //selectedApartment: any;
  constructor() {
  }
  ngOnInit(): void {
    //this.dataSourceObj = new MatTableDataSource<ITransaction>(this.dataList);
 
    //this.dataSourceObj.sort = this.sort;
    //this.transactionService.getTransactions().subscribe(result => {
    //debugger
    //this.dataSourceObj.data = this.dataList;
   // var xxx = dataSourceObj;
    //  //this.dataSource.filter = this.selectedApartment;
    //  this.dataSource.filterPredicate = function (data, filter: string): boolean {
    //    return data.apartmentAddress.toLowerCase().includes(filter);
    //  };


    //  }, error => console.error(error));
}
  ngAfterViewInit(): void {
  //  debugger

  //  var xxx = this.dataList;
  //this.dataSourceObj.sort = this.sort;

}

  doFilter(value: string) {
    this.dataSourceObj.filter = value.trim().toLocaleLowerCase();
  }

  //filter() {
  //  this.dataSourceObj.filter = this.selectedApartment.trim().toLocaleLowerCase();

  //}
  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

}
