import { Component, OnInit, NgZone, ViewChild, Output, Inject, Input, EventEmitter } from "@angular/core";
import { ApartmentService } from "../services/apartment.service";
import { IApartment, ITransaction, IAccount, IBankAccount } from "../models";
import { FormControl, FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { ExpensesService } from "../services/expenses.service";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import { take } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog, MatSnackBar } from "@angular/material";
import { BankAccountService } from "../services/bankAaccount.service";


@Component(
  {
    templateUrl: './expenses-form.component.html',
    selector: 'expenses-form'
  })
export class AddExpenseComponent implements OnInit {

  constructor(private apartmentService: ApartmentService,
    private accountService: AccountService,
    private bankAccountService: BankAccountService,
    private expensesService: ExpensesService,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddExpenseComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  apartments: IApartment[];
  accounts: IAccount[];
  bankAccounts: IBankAccount[];
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
  partnershipApartments: number[] = [1, 3, 4, 20];
  purchaseCostAccounts: number[] = [6, 7, 8, 11, 12, 13, 16, 17, 20];

  @Output() refreshEmitter = new EventEmitter();




  saveTransaction(formValues: any): void {

    if (this.transactionForm.valid) {
      if (this.transactionForm.dirty) {

        let transaction: ITransaction;
        if (this.transactionId == 0) {
          transaction = Object.assign({}, this.transactionForm.value);
          //Since this property could be disabled we have to do this workaround:
          transaction.accountId = this.transactionForm.controls["accountId"].value;
        }
        else {
          transaction = { ...this.data.expense, ...this.transactionForm.value }
        }

        let isPurchaseCost = this.transactionForm.controls['isPurchaseCost'].value;
        transaction.isPurchaseCost = isPurchaseCost;

        //Predict personal transaction id unless it's cash withdrawal account
        if (transaction.accountId != 202) {
          transaction.personalTransactionId = this.calcPersonalTransactionId(transaction);
        }

        //
        if (this.isHourForm) {
          //yp: From 1.7.2024 Stella moves to global salary (no more payments by the hours)
          transaction.amount = 0;//formValues.hours * 10;
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
          //Double transaction for cash
          if (this.data.type == 'receiveCash') {
            this.expensesService.receiveCash(transaction).subscribe((t) => {
              let snackBarRef = this.snackBar.open(`Expense`, `Transaction ID: ${t.id} added`, { duration: 2000 });
              this.refreshEmitter.emit();
            });

          }
          else {
            this.expensesService.addExpense(transaction).subscribe((t) => {
              let snackBarRef = this.snackBar.open(`Expense`, `Transaction ID: ${t.id} added`, { duration: 2000 });
              this.refreshEmitter.emit();
            });

          }
          this.transactionForm.reset();
          this.setSign(0);
        }


      }

    }
  }


  calcPersonalTransactionId(transaction: ITransaction): number {

    //Not relevant for:
    if (transaction.accountId == 200 ||  //for business accounts
      transaction.accountId == 201 ||  //for balance accounts
      (this.isHourForm && transaction.accountId == 4)) //For hourly routine work
    {
      return -3;//not relevant
    }
    else {
      //for partenership apartments no need to cover
      if (this.partnershipApartments.includes(transaction.apartmentId)) {
        return -2;//CoveredByFunds
      }
      else {
        if (transaction.bankAccountId == 0) {
          return 0; //still not covered yet
        }
        else {
          return -1; //investor credit card
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
      isPurchaseCost: { value: false, disabled: false },
      // isPettyCash: true,
      bankAccountId: [0, Validators.min(0)],
    })

    this.configureFormType();
    this.displayTransaction();



    this.transactionForm.get('hours').valueChanges.subscribe(val => {
      if (this.transactionForm.controls['hours'].value) {
        let hours = this.transactionForm.controls['hours'].value;
        this.transactionForm.controls['amount'].setValue(hours * 10);
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

        this.setSign(accountId);
      }
      else {
        this.transactionForm.controls['amount'].setValue(0);
      }
    });


  }

  switchSign() {

    // this.sign = this.sign * -1;
    // this.displySign();
  }


  displySign() {
    // if (this.sign == -1) {

    //   this.signIcone = 'remove';
    //   this.tooltipSign = 'Decrease your account';
    // }
    // else {
    //   this.signIcone = 'add';
    //   this.tooltipSign = 'Increase your account';
    // }
  }


  setSign(accountId: number) {
    if (accountId == 1 || accountId == 198 || accountId == 201 || accountId == 202) {
      this.sign = -1;
    }
    else {
      this.sign = 1;
    }
    // if (accountId == 201 || accountId == 198) {
    //   this.enableSwitch = true;
    // }
    // else {
    //   this.enableSwitch = false;
    // }
    // //Receive cash form can only be negative, hence switch invisible
    // if (this.data.type == "receiveCash") {
    //   this.enableSwitch = false;
    // }
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

    this.bankAccountService.getBankAccounts().subscribe(result => {
      this.bankAccounts = result;
      if (this.data.type == "receiveCash") {
        //Don't allow to select Petty cash
        this.bankAccounts = this.bankAccounts.filter(a => a.id > 0);
      }

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
    else if (this.data.type == "receiveCash") {
      this.labelTitle = "cash withdrawal";
      this.labelDate = "Withdrawal Date";
      this.iconType = "atm"
      this.transactionForm.controls['hours'].clearValidators();
      //Receive cash form can only be negative, hence switch invisible
      this.sign = -1;
      // this.enableSwitch = false;
      this.displySign();
      this.transactionForm.controls['isPurchaseCost'].disable();
      //this.transactionForm.controls['apartmentId'].disable();

      this.transactionForm.controls['accountId'].setValue(202);
      this.transactionForm.controls['accountId'].markAsDirty();
      this.transactionForm.controls['accountId'].disable();

      //remove Petty cash option
      //  this.transactionForm.controls['apartmentId'].setValidators([Validators.required, Validators.min(0), Validators.max(0)]);
      this.transactionForm.controls['bankAccountId'].setValidators([Validators.min(1)]);
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
        bankAccountId: expense.bankAccountId,
      });

      //this.setSign(expense.accountId);
      this.sign = expense.amount < 0 ? -1 : 1;
      //For cash withdrawal - always negative
      if (this.data.expense.accountId == 202) {
        this.sign = -1;
      }
      this.displySign();
    }
    else {
      this.actionName = "Add new";
    }
  }


}



