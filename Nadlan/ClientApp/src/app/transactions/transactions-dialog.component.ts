import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material";
import { SecurityService } from "../security/security.service";
import { TransactionFormComponent } from "./transaction-form/transaction-form.component";
import { ExcelService } from "../services/excel.service";
import { ITransaction } from "../models";




@Component({
  selector: 'transactions-dialog.component',
  templateUrl: './transactions-dialog.component.html',
  styleUrls: ['./transactions-dialog.component.css']

})
export class TransactionsDialogComponent {
  transactionColumns: string[] = ['ptid', 'date', 'amount', 'comments']; /*'id', 'isPurchaseCost' isBusinessExpense*/
  //transactionColumns: string[] = [ 'date', 'amount',  'comments'];
  total: number;
  constructor(
    private dialogRef: MatDialogRef<TransactionsDialogComponent>,
    //private securityService: SecurityService,
    private excelService: ExcelService,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    //if (securityService.hasClaim('admin')) {
    //  this.transactionColumns = ['ptid', 'date', 'amount', 'comments'];
    //}
    let transactions = data.transactions as ITransaction[]
    this.total = transactions.reduce((sum, trans) => sum + trans.amount, 0)
    // let sum = transactions.reduce((total, item)=>total + item.amount,0)
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
  printTotal() {
    if (this.data.transactions) {
      // let trans = this.data.transactions as ITransaction[];
      // this.total = trans.reduce((total,trans)=> total+ trans.amount,0) 
      console.log(this.total);
    }
  }
}
