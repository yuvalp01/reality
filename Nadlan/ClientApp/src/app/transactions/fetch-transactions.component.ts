import { Component, OnInit, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { AppUserAuth } from '../security/app.user.auth';
import { debug } from 'util';
import { strictEqual } from 'assert';

@Component({
  templateUrl: './fetch-transactions.component.html',
  styleUrls: ['./fetch-transactions.component.css']
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'date', 'apartmentId', 'isPurchaseCost', 'accountId', 'amount', 'comments', 'actions'];
  dataSource = new MatTableDataSource<ITransaction>();
  // showConfirmedOnly: boolean = true;
  //allData: ITransaction[];
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  selectedApartment: string = "";
  securityObject: AppUserAuth = null;
  showUnconfirmedOnly: boolean = false;
  showNotCoveredOnly: boolean = false;
  ShowPurchaseCostOnly: boolean = false;

  sum: number = 0;
  constructor(
    private transactionService: TransactionService,
    private dialog: MatDialog,
    //private securityService: SecurityService
  ) {
    //this.securityObject = this.securityService.securityObject;
  }
  ngOnInit(): void {
    this.refreshData();
  }

  refreshData() {
    //refresh tables
    this.transactionService.getTransactions().subscribe(result => {
      // this.allData = result as ITransaction[];
      //  this.filterIsConfirmed();
      //this.dataSource.data = this.allData;
      this.dataSource.data = result as ITransaction[];
      this.dataSource.sort = this.sort;

      this.dataSource.filterPredicate = (data: any, filter: any): boolean => {
        let cleanFilter = filter.substring(1, 100);
        let isMatchApartment = data.apartmentAddress.toLowerCase().includes(cleanFilter);
        let isMatchUnconfirmed = true;
        let isMatchNotCovered = true;
        let isMatchPurchaseCost = true;
        if (this.showNotCoveredOnly) {
          isMatchNotCovered = data.personalTransactionId == -1;
        }
        if (this.showUnconfirmedOnly) {
          isMatchUnconfirmed = data.isConfirmed == false;
        }
        if (this.ShowPurchaseCostOnly) {
          isMatchPurchaseCost = data.isPurchaseCost == true;
        }
        if (isMatchApartment && isMatchUnconfirmed && isMatchNotCovered && isMatchPurchaseCost) {
          this.sum += data.amount;
          return true;
        }
        return false;

      };
    }, error => console.error(error));
  }

  applyFilter(filterValue: string) {
    this.sum = 0;
    this.dataSource.filter = filterValue.trim().toLocaleLowerCase();

  }
  hideShowUnconfirmed(val: any) {
    this.sum = 0;
    this.showUnconfirmedOnly = val.checked;
    //Workaround to trigger the filter predicate:
    this.selectedApartment = 'w' + this.selectedApartment;
    //
    this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();
    this.selectedApartment = this.selectedApartment.substring(1, 100);
  }
  hideShowNotCovered(val: any) {
    this.sum = 0;
    this.showNotCoveredOnly = val.checked;
    //Workaround to trigger the filter predicate:
    this.selectedApartment = 'w' + this.selectedApartment;
    //
    this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();
    this.selectedApartment = this.selectedApartment.substring(1, 100);
  }

  hideShowPurchaseCost(val: any) {
    this.arrangeFilter();
    this.ShowPurchaseCostOnly = val.checked;
    //Workaround to trigger the filter predicate:
  }

  arrangeFilter() {
    this.sum = 0;
    //Workaround to trigger the filter predicate:
    this.selectedApartment = 'w' + this.selectedApartment;   //
    this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();
    this.selectedApartment = this.selectedApartment.substring(1, 100);
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



  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

}

  //ngAfterViewInit(): void {
  //  this.dataSource.sort = this.sort;
  //  // this.dataSource.filter = this.selectedApartment.trim().toLocaleLowerCase();

  //}

  //showUnconfirmed() {
  //  //this.dataSource.data = this.allData.filter(a => !a.isConfirmed);
  //}
  //showAll() {
  //  //this.dataSource.data = this.allData;
  //}



  //filterIsConfirmed() {
  //  //if (this.showConfirmedOnly) {
  //  //  this.dataSource.data = this.allData.filter(a => !a.isConfirmed);
  //  //}
  //  //else {
  //  //  this.dataSource.data = this.allData;
  //  //}
  //}


  //doFilter(value: string) {
  //  this.dataSource.filter = value.trim().toLocaleLowerCase();
  //}

