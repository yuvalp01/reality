import { Component, OnInit } from '@angular/core';
import { ContractService } from '../contract.service';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { ITransaction, IContract, IFilter } from 'src/app/models';
import { TransactionService } from 'src/app/services/transaction.service';
import { TransactionFormComponent } from 'src/app/transactions/transaction-form/transaction-form.component';

@Component({
  selector: 'app-contract-payments',
  templateUrl: './contract-payments.component.html',
  styleUrls: ['./contract-payments.component.css']
})
export class ContractPaymentsComponent implements OnInit {
  contractId: number;
  contract: IContract;
  allTransactions: ITransaction[];
  dataSource = new MatTableDataSource<ITransaction>();
  accountId: any = "1";
  filter: IFilter;
  expectedTran: ITransaction;
  constructor(private contractService: ContractService,
    private transactionService: TransactionService,
    private dialog: MatDialog,
    private route: ActivatedRoute) {
    this.contractId = +this.route.snapshot.paramMap.get('contractId');
    this.filter = {} as IFilter;
    this.filter.monthsBack = 2;
  }

  ngOnInit() {
    this.contractService.getById(this.contractId).subscribe({
      next: (result) => {
        this.contract = result;
        this.loadTransactions()
      },
      error: (err) => console.error(err)
    });

  }

  filterByAccount() {
    // this.accountId = +this.xxx;
    this.dataSource.data = this.allTransactions.filter(a => a.accountId == this.accountId);
  }

  loadTransactions() {
    this.filter.apartmentId = this.contract.apartment.id;
    this.transactionService.getTransactions_(this.filter).subscribe({
      next: (result) => {
        this.allTransactions = result;
        this.filterByAccount();
      },
      error: (err) => console.error(err)
    });
  }

  openTransaction() {
    let expectedTran = this.buildTransaction(this.contract);

    let dialogLocal = this.dialog.open(TransactionFormComponent, {
      height: 'auto',
      width: 'auto',
      data: { transactionId: 0, expected: expectedTran }
    });
    dialogLocal.componentInstance.refreshEmitter.subscribe(() => {
      this.loadTransactions();
      dialogLocal.close();
    });

  }


  buildTransaction(contract: IContract): ITransaction {
    this.expectedTran = {} as ITransaction;
    this.buildCommonProperties();
    this.buildAccountSpecificProperties();
    return this.expectedTran;
  }

  buildCommonProperties() {
    this.expectedTran.id = 0;
    this.expectedTran.accountId = +this.accountId;
    this.expectedTran.apartmentId = this.contract.apartment.id;
    this.expectedTran.date = new Date();
    this.expectedTran.isBusinessExpense = false;
    this.expectedTran.isConfirmed = false;
    this.expectedTran.isPurchaseCost = false;
    this.expectedTran.personalTransactionId = 0;

  }

  buildAccountSpecificProperties() {
    let sharedOwnershipAppartments: number[] = [1,2,3, 4, 20];

    switch (+this.accountId) {
      case 1:
        {
          this.expectedTran.comments = `${this.contract.tenant} rent`;
          this.expectedTran.amount = this.contract.price;
          this.expectedTran.personalTransactionId = -1;
          if (sharedOwnershipAppartments.includes(this.expectedTran.apartmentId)) {
            this.expectedTran.personalTransactionId = -2;
          }
          break;
        }
      case 2:
        {
          this.expectedTran.comments = ``;
          this.expectedTran.amount = this.contract.price * 0.1;
          this.expectedTran.personalTransactionId = 0;
          if(sharedOwnershipAppartments.includes(this.expectedTran.apartmentId))
          {
            this.expectedTran.personalTransactionId = -2;
          }
          break;
        }
      case 50:
        {
          this.expectedTran.comments = `15% income tax`;
          this.expectedTran.amount = this.contract.price * 0.15;
          this.expectedTran.personalTransactionId = -4;
          break;
        }
      default:
        {
          throw ("AccountId does not exist");
        }
    }

  }

  getCurrentDueDate(dueDay: number): Date {
    let dueDate: Date = new Date();
    dueDate.setDate(dueDay);
    return dueDate;
  }

  confirmPayment() {
    this.contract.isPaymentConfirmed = true;
    this.contractService.update(this.contract).subscribe(
      {
        next: () => { },
        error: err => console.error(err)
      });
  }

}
