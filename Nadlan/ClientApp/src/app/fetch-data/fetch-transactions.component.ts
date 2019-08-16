import {Component, OnInit, Output} from '@angular/core';
import { ITransaction } from '../shared/models';
import { TransactionService } from '../services/transaction.service';
import { resource } from 'selenium-webdriver/http';
import { AddTransactionComponent } from '../forms/add-transaction.component';
import { MatDialog } from '@angular/material';
import { subscribeOn } from 'rxjs/operators';

@Component({
    templateUrl:'./fetch-transactions.component.html',
    styles:['table{width:100%}']
})
export class TransactionListComponent implements OnInit
{
    displayedColumns: string[] = ['date','amount', 'apartmentId','accountId','isPurchaseCost','comments'];
    transactions: ITransaction[];
    constructor (private transactionService: TransactionService, private dialog: MatDialog)
    {
    }
    ngOnInit(): void {
        this.transactionService.getTransactions().subscribe(result=>this.transactions=result, error=>console.error(error));
    }
    //dialogRef:any;
    openDialog()
    {
        
         let dialogRef = this.dialog.open(AddTransactionComponent, {
            height: '600px',
            width: '500px',
          }).afterClosed().subscribe(item=>{
            this.transactionService.getTransactions().subscribe(result=>this.transactions=result, error=>console.error(error));
          });

        //   this.dialogRef.componentInstance.test.subscribe(()=>{
        //     console.log('test');
        //   });
    }

}
