import { Component, OnInit, NgZone, ViewChild, Output, Inject, Input } from "@angular/core";
import { ApartmentService } from "../services/apartment.service";
import { IApartment, ITransaction, IAccount } from "../models";
import { FormControl, FormGroup } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { TransactionService } from "../services/transaction.service";
import { Router } from "@angular/router";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import { take } from 'rxjs/operators';
import { EventEmitter } from "events";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
import { debug } from "util";




@Component(
  {
    templateUrl: './add-transaction.component.html',
    selector: 'add-transaction'
  })
export class AddTransactionComponent implements OnInit {

  constructor(private apartmentService: ApartmentService,
    private accountService: AccountService,
    private transactionService: TransactionService,
    private router: Router,
    private _ngZone: NgZone,
    public dialogRef: MatDialogRef<AddTransactionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {

  }
  apartments: IApartment[];
  accounts: IAccount[];
  transactionForm: FormGroup;
  //@Input() isHourForm: boolean = false;
  //doubleTransactionAction: boolean = false;
  labelDate: string = "Transaction Date";
  purchaseCostAccounts: number[] = [6, 7, 8, 11, 12, 13];
  //visibleAccounts: number[] = 

  ngOnInit(): void {
    //debugger
    //if (this.data) {

    //}
    //if (this.data.type == "hours") {
    //  this.isHourForm = true;
    //  this.doubleTransactionAction = true;
    //  this.labelDate = "Date";
    //}
    //else if (this.data.type == "expenses") {
    //  this.doubleTransactionAction = true;
    //}

    this.apartmentService.getApartments().subscribe(result => {
      this.apartments = result;
    }, error => console.error(error));
    this.accountService.getAccounts().subscribe(result => {
      this.accounts = result.filter(a => a.id < 101|| a.id>=199 );
      //if (this.data.visibleAccounts) {

       
      //  //this.accounts = result.filter(a => this.data.visibleAccounts.includes(a.id));
      //  //////Improve naming for clarity
      //  ////this.accounts.find(a => a.id == 4).name = 'Existing Apartment';
      //  ////this.accounts.find(a => a.id == 11).name = 'New Apartment';
      //}
      //else {
      //  this.accounts = result;
      //}
      //if (this.isHourForm) {
      //  //this.accounts = result.filter(a => a.accountTypeId == 0 && a.isIncome == false);
      //  this.accounts = result.filter(a => a.id == 4 || a.id == 6 || a.id == 11);
      //}
      //else if (this.data.visibleAccounts){
      //  this.accounts = result.filter(a => this.data.visibleAccounts.includes(a.id));// a.id == 4 || a.id == 6 || a.id == 11);
      //}
      //else {
      //  this.accounts = result;
      //}

    }, error => console.error(error));

    let apartment = new FormControl(0);
    let account = new FormControl(0);
    let amount = new FormControl();
    let date = new FormControl();
    let hours = new FormControl();
    let comments = new FormControl();
    let isPurchaseCost = new FormControl(false);

    this.transactionForm = new FormGroup(
      {
        date: date,
        hours: hours,
        apartmentId: apartment,
        amount: amount,
        comments: comments,
        isPurchaseCost: isPurchaseCost,
        accountId: account

      });

    //this.transactionForm.get('hours').valueChanges.subscribe(val => {
    //  if (this.transactionForm.controls['hours'].value) {
    //    let hours = this.transactionForm.controls['hours'].value;
    //    this.transactionForm.controls['amount'].setValue(hours * 9);
    //  }
    //  else {
    //    this.transactionForm.controls['amount'].setValue(0);
    //  }
    //});

    this.transactionForm.get('accountId').valueChanges.subscribe(val => {
      if (this.transactionForm.controls['accountId'].value) {
        let accountId = this.transactionForm.controls['accountId'].value;
        //if (accountId == 6 || accountId == 11 ||accountId==12) {
        if (this.purchaseCostAccounts.includes(accountId)) {
          this.transactionForm.controls['isPurchaseCost'].setValue(true);
        }
        else {
          this.transactionForm.controls['isPurchaseCost'].setValue(false);

        }
      }
      else {
        this.transactionForm.controls['amount'].setValue(0);
      }
    });

  }
  saveTransaction(formValues: any): void {

    //this.dialogRef.componentInstance.balanceChanged.emit(null);
    //this.dialogRef.c

    var transaction: ITransaction = Object.assign({}, formValues);
    transaction.hours = 0;
    //if (this.isHourForm) {
    //  transaction.amount = formValues.hours * 9;
    //  //avoid null
    //  transaction.comments = transaction.comments ? transaction.comments : '';
    //  transaction.comments = transaction.comments + `(${formValues.hours}h*9€)`;
    //  //transaction.comments = transaction.comments ? transaction.comments + ` (${formValues.hours}h*9€)` : ` (${formValues.hours}h*9€)`;
    //}
    ///fix UTC issue:
    let date = transaction.date;
    transaction.date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
    ///
    
    //if (this.doubleTransactionAction) {
    //  this.transactionService.addExpense(transaction).subscribe(() => {
    //  });
    //}
    //else {
      this.transactionService.addTransaction(transaction).subscribe(() => {
        //console.log("success!");
        //this.router.navigate(['/fetch-transactions']);
      });
    //}
  }
  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }

  //@Output() balanceChanged = new EventEmitter();
  //  valueChange = new EventEmitter();
  //counter: any = 0;

  //valueChanged() { // You can give any function name
  //  //this.counter = this.counter + 1;
  //  this.balanceChanged.emit('test');
  //  return '';
  //}
}
