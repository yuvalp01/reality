import { Component, Inject } from "@angular/core";
import {  MatDialogRef, MAT_DIALOG_DATA, MatDialog   } from "@angular/material";
import { SecurityService } from "../security/security.service";
import { TransactionFormComponent } from "./transaction-form/transaction-form.component";
import { ExcelService } from "../services/excel.service";
import { debug } from "util";



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
    private excelService: ExcelService,
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
  exportAsXLSX(): void {
    //this.data.data.forEach(a => delete a.id);
    //this.data.data.forEach(a => delete a.stakeholderId);
    this.excelService.exportAsExcelFile(this.data.transactions, 'Transactions');
  }
}
