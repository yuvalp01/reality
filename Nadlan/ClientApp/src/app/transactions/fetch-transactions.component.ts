import { Component, OnInit, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { element } from 'protractor';

@Component({
  templateUrl: './fetch-transactions.component.html',
  styleUrls: ['./fetch-transactions.component.css']
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'date', 'apartmentId', 'isPurchaseCost', 'accountId', 'amount', 'comments', 'actions'];
  dataSource = new MatTableDataSource<ITransaction>();
  showConfirmedOnly: boolean = true;
  allData: ITransaction[];
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  selectedApartment: any;
  constructor(
    private transactionService: TransactionService,
    private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.transactionService.getTransactions().subscribe(result => {
      this.allData = result as ITransaction[];
      this.dataSource.data = this.allData;
      this.dataSource.sort = this.sort;
    }, error => console.error(error));
  }

  refreshData() {
    //refresh tables
    this.transactionService.getTransactions().subscribe(result => {
      this.allData = result as ITransaction[];
      this.filterIsConfirmed();
      //this.dataSource.data = this.allData;
      this.dataSource.sort = this.sort;
      //this.dataSource.filterPredicate = function (data, filter: string): boolean {
      //  return data.apartmentAddress.toLowerCase().includes(filter);
      //};
    }, error => console.error(error));
  }

  showUnconfirmed() {
    this.dataSource.data = this.allData.filter(a => !a.isConfirmed);
  }
  showAll() {
    this.dataSource.data = this.allData;
  }

  hideShowUnconfirmed(val: any) {
    this.showConfirmedOnly = val.checked;
    this.filterIsConfirmed();
  }

  filterIsConfirmed() {
    if (this.showConfirmedOnly) {
      this.dataSource.data = this.allData.filter(a => !a.isConfirmed);
    }
    else {
      this.dataSource.data = this.allData;
    }
  }


  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLocaleLowerCase();

  }
  doFilter(value: string) {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
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




  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

}




  //showUnconfirmed() {
  //  //console.log(value);
  //  //console.log(value.checked);
  //  this.dataSource.filter = value.checked.toString();
  //}
      //this.dataSource.filterPredicate == null;

      //this.dataSource.filterPredicate =
      //  (data: ITransaction, filtersJson: string) => {
      //    const matchFilter = [];
      //    const filters = JSON.parse(filtersJson);

      //    filters.forEach(filter => {
      //      let val = data[filter.id] === null ? '' : data[filter.id];
      //      let filterVal = filter.valuetoLowerCase();
      //      val = val.toLowerCase();
      //      matchFilter.push(val.includes(filterVal));
      //    });
      //    return matchFilter.every(Boolean);
      //  };

      //this.dataSource.filterPredicate = function (data, filter: string): boolean {
      //  let yyy = data.isConfirmed.toString();
      //  return yyy != filter;
      //  //let xxx = data.isConfirmed.includes(filter);
      //  return data.apartmentAddress.toLowerCase().includes(filter);
      //};
    //this.dataSource.filterPredicate = function (data, filter: string): boolean {
    //  let yyy = data.isConfirmed.toString();
    //  return yyy != filter;
    //  //let xxx = data.isConfirmed.includes(filter);
    //  return data.apartmentAddress.toLowerCase().includes(filter);
    //};


    //this.dataSource.filterPredicate = (d: ITransaction, filter: string) => {
    //  const textToSearch = d[column] && d[column].toLowerCase() || '';
    //  return textToSearch.indexOf(filter) !== -1;
    //};

  //  this.dataSource.filterPredicate == null;
  //  this.dataSource.filter = "";

    //const tableFilters = [];
    //tableFilters.push({
    //  id: 'apartmentAddress',
    //  value: filterValue
    //},
    //  //{
    //  //  id: 'comments',
    //  //  value: filterValue
    //  //}

    //);
    //this.dataSource.filter = JSON.stringify(tableFilters);
  //customFilterPredicate() {
  //  this.dataSource.filterPredicate = function (data, filter: string): boolean {
  //    let xxx = data.isConfirmed === filter;
  //    return data.apartmentAddress.toLowerCase().includes(filter);
  //  };
  //}


  //xxx(val: boolean) {
  //  console.log(val);
  //  //this.dataSource.filter = value.trim().toLocaleLowerCase();

  //}
  //onChangeYuv(val: any) {
  //  console.log(val);
  //  console.log(val.checked);
  //}
