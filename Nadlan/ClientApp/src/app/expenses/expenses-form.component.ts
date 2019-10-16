import { Component, OnInit, NgZone, ViewChild, Output, Inject, Input, EventEmitter } from "@angular/core";
import { ApartmentService } from "../services/apartment.service";
import { IApartment, ITransaction, IAccount } from "../models";
import { FormControl, FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { TransactionService } from "../services/transaction.service";
import { Router } from "@angular/router";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import { take } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog, MatSnackBar } from "@angular/material";



@Component(
  {
    templateUrl: './expenses-form.component.html',
    selector: 'expenses-form'
  })
export class AddExpenseComponent implements OnInit {

  constructor(private apartmentService: ApartmentService,
    private accountService: AccountService,
    private transactionService: TransactionService,
    private router: Router,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddExpenseComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  apartments: IApartment[];
  accounts: IAccount[];
  transactionId: number = 0;
  transactionForm: FormGroup;
  @Input() isHourForm: boolean = false;
  labelDate: string;
  labelTitle: string;
  iconType: string;

  purchaseCostAccounts: number[] = [6, 7, 8, 11, 12, 13];

  @Output() refreshEmitter = new EventEmitter();
  //  valueChange = new EventEmitter();
  //counter: any = 0;

  //valueChanged() { // You can give any function name
  //  //this.counter = this.counter + 1;
  //  this.someEvent.emit('test');
  //  return '';
  //}

  saveTransaction(formValues: any): void {


    if (this.transactionForm.valid) {
      if (this.transactionForm.dirty) {

        var transaction: ITransaction = Object.assign({}, formValues);
        if (this.isHourForm) {
          transaction.amount = formValues.hours * 9;
        }
        else {
          transaction.hours = 0;
        }
        ///fix UTC issue:
        //let date = transaction.date;
        let date = new Date(transaction.date);
        transaction.date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
        ///

        if (this.transactionId > 0) {
          transaction.id = this.transactionId;
          this.transactionService.updateExpense(transaction).subscribe(() => {
            let snackBarRef = this.snackBar.open(`Expense`, 'Updated', { duration: 2000 });
            this.refreshEmitter.emit();
            //console.log("success!");
            //this.dialogRef.close("added!");
            //this.router.navigate(['/expenses']);
          });

        }
        else {
          this.transactionService.addExpense(transaction).subscribe(() => {
            let snackBarRef = this.snackBar.open(`Expense`, 'Added', { duration: 2000 });
            //this.transactionForm.reset();
            this.refreshEmitter.emit();
            //console.log("success!");
            //this.dialogRef.close("added!");
            //this.router.navigate(['/expenses']);
          });
        }
       
      }

    }
  }


  ngOnInit(): void {

    this.getAllLists();

    this.transactionForm = this.formBuilder.group({
      apartmentId: 0,
      accountId: [0, Validators.min(1)],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      hours: null,
      comments: ['', Validators.required],
      isPurchaseCost: false,
    })

    this.configureFormType();
    this.displayTransaction();



    this.transactionForm.get('hours').valueChanges.subscribe(val => {
      if (this.transactionForm.controls['hours'].value) {
        let hours = this.transactionForm.controls['hours'].value;
        this.transactionForm.controls['amount'].setValue(hours * 9);
      }
      else {
        this.transactionForm.controls['amount'].setValue(0);
      }
    });

    this.transactionForm.get('accountId').valueChanges.subscribe(val => {
      if (this.transactionForm.controls['accountId'].value) {
        let accountId = this.transactionForm.controls['accountId'].value;
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

  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }


  private getAllLists() {
    this.apartmentService.getApartments().subscribe(result => {
      this.apartments = result;
    }, error => console.error(error));
    this.accountService.getAccounts().subscribe(result => {
      if (this.data.visibleAccounts) {
        this.accounts = result.filter(a => this.data.visibleAccounts.includes(a.id));
      }
      else {
        this.accounts = result;
      }

    }, error => console.error(error));
  }

  configureFormType() {
    if (this.data.type == "hours") {
      this.isHourForm = true;
      //this.doubleTransactionAction = true;
      this.labelTitle = "activity";
      this.labelDate = "Activity Date";
      this.iconType = "timer"
      this.transactionForm.controls['hours'].setValidators(Validators.required);
    }
    else if (this.data.type == "expenses") {
      this.labelTitle = "axpense";
      this.labelDate = "Payment Date";
      this.iconType = "attach_money"
      this.transactionForm.controls['hours'].clearValidators();

      //this.doubleTransactionAction = true;
    }
    else {
      throw "form type not implemented yet"
    }
    this.transactionForm.updateValueAndValidity();
  }

  displayTransaction() {
    //if (this.transactionForm) {
    //  this.transactionForm.reset();
    //}
    //debugger
    if (this.data.expense) {
      let expense: ITransaction = this.data.expense;
      this.transactionId = expense.id;
      this.transactionForm.setValue({
        apartmentId: expense.apartmentId,
        accountId: expense.accountId,
        amount: expense.amount,
        date: expense.date,
        hours: expense.hours,
        comments: expense.comments,
        isPurchaseCost: expense.isPurchaseCost,
      });
      //apartment.setValue(expense.apartmentId);
      //account.setValue(expense.accountId);
      //amount.setValue(expense.amount);
      //date.setValue(expense.date);
      //hours.setValue(expense.hours);
      //comments.setValue(expense.comments);
      //isPurchaseCost.setValue(expense.isPurchaseCost);
    }
  }
}





