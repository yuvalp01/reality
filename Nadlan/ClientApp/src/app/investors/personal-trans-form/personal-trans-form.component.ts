import { Component, OnInit, ViewChild, NgZone, Inject, Output, EventEmitter, ɵChangeDetectorStatus } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IStakeholder, IPersonalTransaction, IApartment, ITransaction, IFilter, IPersonalTransWithFilter } from '../../models';
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
  dataSourceTrans = new MatTableDataSource<ITransaction>();
  //displayedColumns: string[] = ['id', 'date', 'apartmentId', 'isPurchaseCost', 'accountId', 'amount', 'comments', 'actions'];
  displayedColumns: string[] = ['id', 'date', 'amount', 'comments', 'account'];
  years: number[] = [2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026];

  isFilterConfirmed: boolean = false;
  showResults: boolean = false;

  //filterTransForm: FormGroup;
  filterTrans: IFilter;

  @Output() refreshEmitter = new EventEmitter();
  @Output() chageStakeholderEmitter = new EventEmitter();

  ngOnInit() {
    this.personalTransForm = this.formBuilder.group({
      stakeholderId: [0, Validators.min(1)],
      apartmentId: [0, Validators.min(-2)],
      transactionType: [null, Validators.required],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      comments: ['', Validators.required],
      // showPurchaseCostOnly: false,
      filter_isPurchaseCost: null,
      filter_year: null,

    });

    // this.filterTransForm = this.formBuilder.group({
    //   isPurchaseCost: null,
    //   year: null,
    // });


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

          //let isConfirmed = this.personalTransForm.controls.isConfirmed.value;
          if (this.isFilterConfirmed && this.dataSourceTrans.data.length > 0) {
            // let filter = {} as IFilter;
            // filter.apartmentId = t.apartmentId;
            // filter.personalTransactionId = 0;
            // filter.isPurchaseCost = this.personalTransForm.controls.showPurchaseCostOnly.value;

            let transWithFilter = {} as IPersonalTransWithFilter;
            transWithFilter.filter = this.filterTrans;
            transWithFilter.personalTransaction = t;
            this.personalTransService.addPersonalTransWithFilter(transWithFilter)
              .subscribe({
                next: result => this.onSaveComplete_(result),
                error: err => console.error(err)
              });
          }
          //block
          else {
            this.personalTransService.addPersonalTrans(t)
              .subscribe({
                next: result => this.onSaveComplete(result),
                error: err => console.error(err)
              });
          }
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


  onSaveComplete_(result: number) {
    let action = 'Updated';
    if (result) {
      // this.personalTrans = result;
      action = `Added with ${result} affected`;
    }
    let snackBarRef = this.snackBar.open(`Personal transaction`, action, { duration: 2000 });
    this.refreshEmitter.emit();
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
  closeResults(isConfirmed) {
    this.showResults = false;
    this.isFilterConfirmed = isConfirmed;
    if (isConfirmed) {
      let sum = this.dataSourceTrans.data.reduce((sum, current) => sum + current.amount, 0)
      // this.totalTransactions = this.dataSourceTrans.data.length;
      this.personalTransForm.controls.amount.setValue(sum);
    }
    else {
      this.personalTransForm.controls.filter_year.reset();
      this.personalTransForm.controls.filter_isPurchaseCost.reset();
      this.dataSourceTrans = new MatTableDataSource<ITransaction>();
    }

  }


  loadFilter() {
    this.filterTrans = {} as IFilter;
    this.filterTrans.isSoFar = true;
    this.filterTrans.personalTransactionId = 0;
    this.filterTrans.apartmentId = this.personalTransForm.controls.apartmentId.value;
    if (this.personalTransForm.controls.filter_year.value) {
      this.filterTrans.year = this.personalTransForm.controls.filter_year.value;
    }
    if (this.personalTransForm.controls.filter_isPurchaseCost.value) {
      this.filterTrans.isPurchaseCost = this.personalTransForm.controls.filter_isPurchaseCost.value;
    }
    //We do not charge business transactions:
    this.filterTrans.isBusinessExpense = false;
  }

  loadTransactions() {
    this.loadFilter();
    this.showResults = true;
    // let filter: IFilter = {} as IFilter;
    // filter.personalTransactionId = 0;
    // filter.apartmentId = this.personalTransForm.controls.apartmentId.value;
    // filter.isPurchaseCost = this.personalTransForm.controls.showPurchaseCostOnly.value;
    // debugger
    // if (this.personalTransForm.controls.purchaseCost.value != -1) {
    //   filter.isPurchaseCost = this.personalTransForm.controls.purchaseCost.value;
    // }
    this.transactionService.getFilteredTransactions(this.filterTrans).subscribe({
      next: (result) => {
        this.dataSourceTrans.data = result;
        // let total = this.dataSourceTrans.data.reduce((sum, current) => sum + current.amount, 0)
        // this.personalTransForm.controls.amount.setValue(total);
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
