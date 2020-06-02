import { Component, OnInit, NgZone, ViewChild, Output, Inject, Input, EventEmitter } from "@angular/core";
import { ApartmentService } from "../services/apartment.service";
import { IApartment, ITransaction, IAccount } from "../models";
import { FormControl, FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { ExpensesService } from "../services/expenses.service";
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
    private expensesService: ExpensesService,
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
  actionName: string;
  iconType: string;
  sign: number = 1;
  signIcone: string = 'add';
  tooltipSign: string = 'Increase your account';
  enableSwitch: boolean = false;

  purchaseCostAccounts: number[] = [6, 7, 8, 11, 12, 13, 16, 17,18];

  @Output() refreshEmitter = new EventEmitter();

  saveTransaction(formValues: any): void {


    if (this.transactionForm.valid) {
      if (this.transactionForm.dirty) {

        var transaction: ITransaction = Object.assign({}, this.transactionForm.value);
        let isPurchaseCost = this.transactionForm.controls['isPurchaseCost'].value;
        transaction.isPurchaseCost = isPurchaseCost;
        //In the future, add a checkbox "use investor credit card" 
        //transaction.personalTransactionId = -1;
        transaction.personalTransactionId = 0;//still not covered
        //
        if (this.isHourForm) {
          transaction.amount = formValues.hours * 9;
        }
        else {
          transaction.hours = 0;
        }
        transaction.amount = this.sign * transaction.amount;
        ///fix UTC issue:
        let date = new Date(transaction.date);
        transaction.date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
        ///

        if (this.transactionId > 0) {
          transaction.id = this.transactionId;
          this.expensesService.updateExpense(transaction).subscribe(() => {
            let snackBarRef = this.snackBar.open(`Expense`, 'Updated', { duration: 2000 });
            this.refreshEmitter.emit();
          });

        }
        else {
          this.expensesService.addExpense(transaction).subscribe(() => {
            let snackBarRef = this.snackBar.open(`Expense`, 'Added', { duration: 2000 });
            this.refreshEmitter.emit();
          });
          this.transactionForm.reset();
          this.setSign(0);
        }


      }

    }
  }


  ngOnInit(): void {

    this.getAllLists();

    this.transactionForm = this.formBuilder.group({
      apartmentId: 0,
      accountId: [0, Validators.min(1)],
      amount: [null, [Validators.required, Validators.min(0)]],
      date: [null, Validators.required],
      hours: null,
      comments: ['', Validators.required],
      isPurchaseCost: { value: false, disabled: true },
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
        if (accountId==201) {
          this.transactionForm.controls['apartmentId'].setValue(0);
          this.transactionForm.controls['apartmentId'].disable();
        }
        else {
          this.transactionForm.controls['apartmentId'].enable();

        }
        this.setSign(accountId);
      }
      else {
        this.transactionForm.controls['amount'].setValue(0);
      }
    });

  }

  switchSign() {
    //this.opositSign = this.opositSign * -1;
    this.sign = this.sign * -1;
    this.displySign();
  }


  displySign() {
    if (this.sign == -1) {

      this.signIcone = 'remove';
      this.tooltipSign = 'Decrease your account';
    }
    else {
      this.signIcone = 'add';
      this.tooltipSign = 'Increase your account';
    }
  }


  setSign(accountId: number) {
    if (accountId == 1 || accountId==198 || accountId == 201) {
      this.sign = -1;      
    }
    if (accountId== 201|| accountId==198) {
      this.enableSwitch = true;
    }
    else {
      this.enableSwitch = false;
    }
    this.displySign();
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
      this.labelTitle = "activity";
      this.labelDate = "Activity Date";
      this.iconType = "timer"
      this.transactionForm.controls['hours'].setValidators([Validators.required, Validators.min(0.1)]);
    }
    else if (this.data.type == "expenses") {
      this.labelTitle = "expense";
      this.labelDate = "Payment Date";
      this.iconType = "attach_money"
      this.transactionForm.controls['hours'].clearValidators();

    }
    else {
      throw "form type not implemented yet"
    }
    this.transactionForm.updateValueAndValidity();
  }

  displayTransaction() {

    if (this.data.expense) {
      this.actionName = "Edit";
      let expense: ITransaction = this.data.expense;
      this.transactionId = expense.id;

      this.transactionForm.setValue({
        apartmentId: expense.apartmentId,
        accountId: expense.accountId,
        amount: Math.abs(expense.amount),
        date: expense.date,
        hours: expense.hours,
        comments: expense.comments,
        isPurchaseCost: expense.isPurchaseCost,
      });

      //this.setSign(expense.accountId);
      this.sign = expense.amount < 0 ? -1 : 1;
      this.displySign();
    }
    else {
      this.actionName = "Add new";
    }
  }


}





