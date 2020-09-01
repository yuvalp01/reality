import { Component, OnInit } from '@angular/core';
import { ContractService } from '../contract.service';
import { MatTableDataSource, MatDialog, MatSnackBar } from '@angular/material';
import { IContract, ITransaction } from 'src/app/models';
import { ContractFormComponent } from '../contract-form/contract-form.component';
import { TransactionFormComponent } from 'src/app/transactions/transaction-form/transaction-form.component';

@Component({
  selector: 'app-contract-list',
  templateUrl: './contract-list.component.html',
  styleUrls: ['./contract-list.component.css']
})
export class ContractListComponent implements OnInit {

  constructor(private contractService: ContractService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog) { }
  dataSource = new MatTableDataSource<IContract>();
  displayedColumns: string[] = [
   'apartment',
   'tenant', 
   'price', 
   'dateStart', 
   'dateEnd', 
   'isElectriciyChanged',
   'isPaymentConfirmed',
   'actions'
  ]
  ngOnInit() {
    this.loadItems();
  }

  loadItems() {
    this.contractService.getAll().subscribe({
      next: (result) => this.dataSource.data = result,
      error: (err) => console.error(err)
    })
  }

  openForm(item: IContract) {
    let dialogLocal = this.dialog.open(ContractFormComponent, {
      height: 'auto',
      width: 'auto',
      data: item
    });
    dialogLocal.afterClosed().subscribe(() => this.loadItems())
  }
  delete(id: number) {
    if (confirm("Are you sure you want to delete?")) {
      this.contractService.delete(id).subscribe({
        next: () => {
          this.snackBar.open(`Contract id ${id}`, 'Deleted', { duration: 2000 });
          this.loadItems();
        },
        error: (err) => console.error(err)
      })
    }
  }
  openTransaction(contract: IContract) {
    let expectedTran = this.buildTransaction(contract);
    let dialogLocal = this.dialog.open(TransactionFormComponent, {
      height: 'auto',
      width: 'auto',
      data: { transactionId: 0, expected:expectedTran }
    });
    dialogLocal.afterClosed().subscribe(() => this.loadItems())
  }


  buildTransaction(contract:IContract):ITransaction
  {
    const expectedTran = {} as ITransaction;
    expectedTran.id = 0;  
    expectedTran.accountId = 1;
    expectedTran.apartmentId = contract.apartment.id;
    expectedTran.date = new Date();
    expectedTran.isBusinessExpense = false;
    expectedTran.isConfirmed = false;
    expectedTran.isPurchaseCost = false;
    expectedTran.comments = `${contract.tenant} rent`;
    expectedTran.amount = contract.price;
    expectedTran.personalTransactionId = 0;
    return expectedTran;
    
  }

  complete(event: any, item: IContract) {
    item.isPaymentConfirmed = event.checked;
    this.contractService.update(item).subscribe(
      {
        next: () => this.loadItems(),
        error: err => console.error(err)
      });
  }
}
