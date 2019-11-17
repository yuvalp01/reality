import { Component, Inject } from "@angular/core";
import {  MatDialogRef, MAT_DIALOG_DATA   } from "@angular/material";
import { ITransaction } from "../models";



@Component({
  selector: 'transactions-dialog.component',
  templateUrl: './transactions-dialog.component.html',

})
export class TransactionsDialogComponent {
  transactionColumns: string[] = ['date', 'amount', 'comments']; /*'id', 'isPurchaseCost'*/
  //transactionColumns: string[] = [ 'date', 'amount',  'comments'];

  constructor(
    private dialogRef: MatDialogRef<TransactionsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }
}
