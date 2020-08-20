import { Component, OnInit, ViewChild } from '@angular/core';
import { ITransaction } from '../models';
import { TransactionService } from '../services/transaction.service';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { AppUserAuth } from '../security/app.user.auth';
import { element } from 'protractor';
import { MessageBoxComponent } from '../issues/message-box/message-box.component';
import { SecurityService } from '../security/security.service';


@Component({
  templateUrl: './fetch-transactions.component.html',
  styleUrls: ['./fetch-transactions.component.css']
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'date', 'apartmentId', 'isPurchaseCost', 'accountId', 'amount', 'comments', 'actions'];
  dataSource = new MatTableDataSource<ITransaction>();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  selectedApartment: string = "";
  securityObject: AppUserAuth = null;
  showUnconfirmedOnly: boolean = false;
  showNotCoveredOnly: boolean = false;
  showPurchaseCostOnly: boolean = false;
  showSharedApartmentsOnly: boolean = false;
  selectedApartmentStatus: number = 0;
  readonly sharedApartments:number[] = [1,3,4,20];
  sum: number = 0;
  monthsBack:number=3;
  currentUser: string = 'unknown';

  constructor(
    private transactionService: TransactionService,
    private securityService: SecurityService,
    private dialog: MatDialog,
  ) {

  }
  ngOnInit(): void {
    if (this.securityService.securityObject.userName != '') {
      this.currentUser = this.securityService.securityObject.userName;
    }
    this.refreshData();
  }
  loadAllList()
  {
    this.monthsBack = 0;
    this.refreshData();
  }
  refreshData() {
    //refresh tables
    this.transactionService.getTransactions(this.monthsBack).subscribe(result => {

      this.dataSource.data = result as ITransaction[];
      this.dataSource.sort = this.sort;


      this.dataSource.filterPredicate = (data: any, filter: any): boolean => {
        let cleanFilter = filter.substring(1, 100);
        let isMatchApartment = data.apartmentAddress.toLowerCase().includes(cleanFilter);
        let isMatchUnconfirmed = true;
        let isMatchNotCovered = true;
        let isMatchPurchaseCost = true;
        let isMatchSharedApartmentsOnly = true;
        let isMatchShowAllStatuses = true;

        if (this.selectedApartmentStatus == 0) {
          isMatchShowAllStatuses = true;
        }
        else {
          if (this.selectedApartmentStatus == 100) {
            if (data.apartmentStatus == 100) {
              isMatchShowAllStatuses = true;
            }
            else {
              isMatchShowAllStatuses = false;
            }
          }
          if (this.selectedApartmentStatus == -1) {
            if (data.apartmentStatus != 100) {
              isMatchShowAllStatuses = true;
            }
            else {
              isMatchShowAllStatuses = false;
            }

          }
        }

        if (this.showNotCoveredOnly) {
          isMatchNotCovered = data.personalTransactionId == 0;
        }
        if (this.showSharedApartmentsOnly) {
          isMatchSharedApartmentsOnly = this.sharedApartments.includes(data.apartmentId);
        }
        

        if (this.showUnconfirmedOnly) {
          isMatchUnconfirmed = data.isConfirmed == false;
        }
        if (this.showPurchaseCostOnly) {
          isMatchPurchaseCost = data.isPurchaseCost == true;
        }
        if (isMatchApartment &&
          isMatchUnconfirmed &&
          isMatchNotCovered &&
          isMatchPurchaseCost &&
          isMatchSharedApartmentsOnly &&
          isMatchShowAllStatuses) {
          this.sum += data.amount;
          return true;
        }
        return false;

      };
      this.checkNewMessages();
    }, error => console.error(error));
  }

  applyFilter(filterValue: string) {
    this.sum = 0;
    this.dataSource.filter = filterValue.trim().toLocaleLowerCase();

  }

  refreshFilter() {
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

  openMessages(id: number) {

    let dialogLocal = this.dialog.open(MessageBoxComponent, {
      height: 'auto',
      width: 'auto',
      data: { tableName: 'transactions', id: id }
    });
   // dialogLocal.afterClosed().subscribe(() => this.loadList())
    // dialogLocal.componentInstance.refreshEmitter.subscribe(() => this.loadList())
  }

  checkNewMessages() {
    this.dataSource.data.forEach(trans => {
      if (trans.messages.length == 0) {
        trans['hasUnread'] = false;
      }
      else {
        let unread = trans.messages.filter(a => a.userName.toLowerCase() != this.currentUser && !a.isRead);
        if (unread.length > 0) trans['hasUnread'] = true;
        else trans['hasUnread'] = false;
      }
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

