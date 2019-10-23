import { Component, Inject, Input } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { IPersonalTransaction } from "../../models";
import { debug } from "util";



@Component({
  styleUrls: ['./personal-trans-dialog.component.css'],
  templateUrl: './personal-trans-dialog.component.html',

})
export class PersonalTransDialogComponent {
  transactionColumns: string[] = ['date', 'amount', 'comments'];
  //transactionColumns: string[] = [ 'date', 'amount',  'comments'];

  constructor(private dialogRef: MatDialogRef<PersonalTransDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
    if (data.columns) {
      this.transactionColumns = data.columns;
    }
  }
}
