import { Component, Inject } from "@angular/core";
import {  MatDialogRef, MAT_DIALOG_DATA, MatDialog   } from "@angular/material";
import { SecurityService } from "../security/security.service";
import { TransactionFormComponent } from "./transaction-form/transaction-form.component";



@Component({
  selector: 'transactions-dialog.component',
  templateUrl: './transactions-dialog.component.html',
  styleUrls:['./transactions-dialog.component.css']

})
export class TransactionsDialogComponent {
  transactionColumns: string[] = ['ptid','date', 'amount', 'comments']; /*'id', 'isPurchaseCost' isBusinessExpense*/
  //transactionColumns: string[] = [ 'date', 'amount',  'comments'];

  constructor(
    private dialogRef: MatDialogRef<TransactionsDialogComponent>,
    //private securityService: SecurityService,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    //if (securityService.hasClaim('admin')) {
    //  this.transactionColumns = ['ptid', 'date', 'amount', 'comments'];
    //}
  }

  openForm(_transactionId: number) {
    console.log(_transactionId);
    let dialogRef = this.dialog.open(TransactionFormComponent, {
      height: '600px',
      width: '500px',
      data: { transactionId: _transactionId }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => { });
  }
}
