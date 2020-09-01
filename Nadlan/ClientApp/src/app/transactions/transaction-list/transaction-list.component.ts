import { Component, OnInit, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { ITransaction } from '../../models/basic';
import { TransactionService } from '../../services/transaction.service';
import { UtilitiesService } from 'src/app/services/utilities.service';

@Component({
  selector: 'transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent_ implements OnInit {

  @Input() dataSource;// = new MatTableDataSource<ITransaction>();
  @Input() displayedColumns: string[];// = ['apartmentId', 'amount', 'date'];
  constructor(private transactionService: TransactionService,
    public utilitiesService: UtilitiesService) { }


  ngOnInit() {
    // this.transactionService.getTransactions(1).subscribe({
    //   next: (result) => this.dataSource.data = result,
    //   error: (err) => console.error(err)
    // });
  }
  public isPositive(value: number): boolean {
    return this.utilitiesService.isPositive(value);
  }
}
