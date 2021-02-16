import { Component, OnInit, ViewChild, NgZone, Inject, Output, EventEmitter, ɵChangeDetectorStatus } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IStakeholder, IPersonalTransaction, IApartment, ITransaction, IFilter } from '../../models';
import { PersonalTransService } from '../personal-trans.service';
import { MAT_DIALOG_DATA, MatSnackBar, MatTableDataSource } from '@angular/material';
import { ApartmentService } from 'src/app/services/apartment.service';
import { TransactionService } from 'src/app/services/transaction.service';



@Component({
  selector: 'app-personal-trans-form',
  templateUrl: './personal-trans-form.component.html',
  styleUrls: ['./personal-trans-form.component.css']
})
export class PersonalTransFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private personalTransService: PersonalTransService,
    private transactionService: TransactionService,
    private apartmentService: ApartmentService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  title: string;
  personalTransForm: FormGroup;
  stakeholders: IStakeholder[];
  apartments: IApartment[];
  personalTrans: IPersonalTransaction;
  //showPurchaseCostOnly: boolean = false;
  dataSourceTrans = new MatTableDataSource<ITransaction>();
  //displayedColumns: string[] = ['id', 'date', 'apartmentId', 'isPurchaseCost', 'accountId', 'amount', 'comments', 'actions'];
  displayedColumns: string[] = ['id', 'date', 'amount', 'comments', 'account'];

  //transactions: ITransaction[];

  @Output() refreshEmitter = new EventEmitter();
  @Output() chageStakeholderEmitter = new EventEmitter();

  ngOnInit() {
    this.personalTransForm = this.formBuilder.group({
      stakeholderId: [0, Validators.required],
      apartmentId: [0, Validators.min(-2)],
      transactionType: [null, Validators.required],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      comments: ['', Validators.required],
      showPurchaseCostOnly: false,
    });

    this.loadData();

    this.personalTransForm.get('stakeholderId').valueChanges.subscribe(val => {
      let currentStakeholder = this.personalTransForm.controls['stakeholderId'].value;
      this.chageStakeholderEmitter.emit(currentStakeholder)
    });

  }

  saveTransaction() {

    if (this.personalTransForm.valid) {
      if (this.personalTransForm.dirty) {
        const t: IPersonalTransaction = { ...this.personalTrans, ...this.personalTransForm.value }
        t.date = this.fixUtcDate(t.date);

        if (t.id) {
          this.personalTransService.editPersonalTrans(t)
            .subscribe({
              next: result => this.onSaveComplete(result),
              error: err => console.error(err)
            })
        }
        else {
          this.personalTransService.addPersonalTrans(t)
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


  onSaveComplete(result: IPersonalTransaction) {
    let action = 'Updated';
    if (result) {
      this.personalTrans = result;
      action = 'Added';
    }
    let snackBarRef = this.snackBar.open(`Personal transaction`, action, { duration: 2000 });
    this.refreshEmitter.emit();
  }



  loadData() {
    this.personalTransService.getStakeholders()
      .subscribe({
        next: result => this.stakeholders = result,
        error: err => console.error(err)
      });
    if (this.data.transactionId == 0) {
      this.title = "Add new";
      if (this.data.expected) {

        this.personalTransForm.patchValue(this.data.expected);

      }

    }
    else {
      this.title = "Edit";
      //this.loadTrans(this.data.transactionId);
      this.personalTransService.getPesonalTransById(this.data.transactionId)
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

  loadTrans(result: IPersonalTransaction) {
    this.personalTrans = result;
    this.personalTransForm.patchValue(this.personalTrans);
    this.personalTransForm.get('stakeholderId').disable();;
  }




  public fixUtcDate(dateIn) {
    ///fix UTC issue:
    let date = new Date(dateIn);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 3);
    ///
  }


  loadTransactions() {
    let filter: IFilter = {} as IFilter;
    filter.personalTransactionId = 0;
    filter.apartmentId = this.personalTransForm.controls.apartmentId.value;
    filter.isPurchaseCost = this.personalTransForm.controls.showPurchaseCostOnly.value;
    this.transactionService.getTransactions_(filter).subscribe({
      next: (result) => {
        this.dataSourceTrans.data = result;
        let total = this.dataSourceTrans.data.reduce((sum, current) => sum + current.amount, 0)
        this.personalTransForm.controls.amount.setValue(total);
      },
      error: (err) => console.error(err)
    });
  }


  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }

  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }
}
