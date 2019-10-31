import { Component, OnInit, ViewChild, NgZone, Inject, Output, EventEmitter, ɵChangeDetectorStatus } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IStakeholder, IPersonalTransaction, IApartment } from '../../models';
import { PersonalTransService } from '../personal-trans.service';
import { MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { ApartmentService } from 'src/app/services/apartment.service';



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
    private apartmentService: ApartmentService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  title: string;
  personalTransForm: FormGroup;
  stakeholders: IStakeholder[];
  apartments: IApartment[];
  personalTrans: IPersonalTransaction;
  @Output() refreshEmitter = new EventEmitter();
  @Output() chageStakeholderEmitter = new EventEmitter();

  ngOnInit() {
    this.loadData();
    this.personalTransForm = this.formBuilder.group({
      stakeholderId: [0, Validators.min(-2)],
      apartmentId: [0, Validators.min(-2)],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      comments: ['', Validators.required],
    });
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
    //let stakeholderIdControl =
    //stakeholderIdControl.disable();
  }




  //initializeNewTransaction(): IPersonalTransaction {

  //  return {
  //    id: 0,
  //    Amount: null,
  //    Comments: '',
  //    date: null,
  //    StakeholderId: null
  //  }

  //}

  public fixUtcDate(dateIn) {
    ///fix UTC issue:
    let date = new Date(dateIn);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
    ///
  }



  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
