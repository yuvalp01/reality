import { Component, OnInit, ViewChild, NgZone, Inject, Output, EventEmitter, ɵChangeDetectorStatus } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IApartment, IAccount, ITransaction, IBankAccount } from '../../models';
import { TransactionService } from '../../services/transaction.service';
import { MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { ApartmentService } from '../../services/apartment.service';
import { AccountService } from '../../services/account.service';
import { BankAccountService } from 'src/app/services/bankAaccount.service';


@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private transactionService: TransactionService,
    private accountService: AccountService,
    private bankAccountService: BankAccountService,
    private apartmentService: ApartmentService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  title: string;
  transactionFormGroup: FormGroup;
  accounts: IAccount[];
  bankAccounts: IBankAccount[];
  apartments: IApartment[];
  transaction: ITransaction;
  @Output() refreshEmitter = new EventEmitter();


  ngOnInit() {

    this.transactionFormGroup = this.formBuilder.group({
      accountId: [0, Validators.min(-2)],
      apartmentId: [0, Validators.min(-2)],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      isPurchaseCost: [false],
      isBusinessExpense: [false],
      isConfirmed: [false],
      personalTransactionId: [null, Validators.required],
      comments: [''],
      includeAllRentTrans: [false],
      bankAccountId: [-1, Validators.min(0)],
    });
    this.loadData();
  }

  loadData() {
    this.accountService.getAccounts()
      .subscribe({
        next: result => this.accounts = result,
        error: err => console.error(err)
      });

    this.bankAccountService.getBankAccounts().subscribe(result => {
      this.bankAccounts = result;
    }, error => console.error(error));

    if (this.data.transactionId == 0) {
      this.title = "Add new";
      if (this.data.expected) {
        this.loadTrans(this.data.expected);
        this.transactionFormGroup.markAsDirty();
      }
    }
    else {
      this.title = "Edit";
      this.transactionService.getTransactionById(this.data.transactionId)
        .subscribe({
          next: result => {
            this.loadTrans(result)
          },
          error: err => console.error(err)
        });
    }

    this.apartmentService.getApartments()
      .subscribe({
        next: result => this.apartments = result,
        error: err => console.error(err)
      });

  }

  loadTrans(result: ITransaction) {
    this.transaction = result;
    this.transactionFormGroup.patchValue(this.transaction);
    if (this.transaction.accountId == 1) {
      this.transactionFormGroup.controls['includeAllRentTrans'].setValue(true);
    }
  }



  saveTransaction() {

    if (this.transactionFormGroup.valid) {
      if (this.transactionFormGroup.dirty) {
        const t: ITransaction = { ...this.transaction, ...this.transactionFormGroup.value }
        t.date = this.fixUtcDate(t.date);
        let includeAllRentTrans = this.transactionFormGroup.value.includeAllRentTrans;

        if (t.id) {
          this.transactionService.updateTransaction(t, includeAllRentTrans)
            .subscribe({
              next: result => this.onSaveComplete(result),
              error: err => console.error(err)
            })
        }
        else {
          this.transactionService.addTransaction(t, includeAllRentTrans)
            .subscribe({
              next: result => this.onSaveComplete(result),
              error: err => console.error(err)
            });

        }
      }
      else {
        //no change
        return;
      }
    }
    else {
      //Not valid
      return;
    }
  }


  onSaveComplete(result: ITransaction) {
    let action = 'Updated';
    if (result) {
      this.transaction = result;
      this.transaction.id = 0;
      action = 'Added';
    }
    let snackBarRef = this.snackBar.open(`Transaction`, action, { duration: 2000 });
    this.refreshEmitter.emit();
  }


  public fixUtcDate(dateIn) {
    ///fix UTC issue:
    let date = new Date(dateIn);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 3);
    ///
  }

  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
