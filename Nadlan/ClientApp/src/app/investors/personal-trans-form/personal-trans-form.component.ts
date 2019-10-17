import { Component, OnInit, ViewChild, NgZone, Inject, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IStakeholder, IPersonalTransaction } from '../../models';
import { PersonalTransService } from '../../services/personal-trans.service';
import { MAT_DIALOG_DATA } from '@angular/material';



@Component({
  selector: 'app-personal-trans-form',
  templateUrl: './personal-trans-form.component.html',
  styleUrls: ['./personal-trans-form.component.css']
})
export class PersonalTransFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private _ngZone: NgZone,
    private personalTransService: PersonalTransService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  title: string;
  personalTransForm: FormGroup;
  stakeholders: IStakeholder[];
  personalTrans: IPersonalTransaction;
  @Output() refreshEmitter = new EventEmitter();

  ngOnInit() {
    this.loadData();
    this.personalTransForm = this.formBuilder.group({
      stakeholderId: [0, Validators.min(1)],
      amount: [null, Validators.required],
      date: [null, Validators.required],
      comments: ['', Validators.required],
    });
  }

  saveTransaction() {

    if (this.personalTransForm.valid) {
      if (this.personalTransForm.dirty) {
        const t: IPersonalTransaction = { ...this.personalTrans, ...this.personalTransForm.value }
        t.date = this.fixUtcDate(t.date);

        if (t.id == 0) {
          this.personalTransService.addPersonalTrans(t)
            .subscribe({
              next: result => this.onSaveComplete(result),
              error: err => console.error(err)
            });
        }
        else {
          this.personalTransService.editPersonalTrans(t)
            .subscribe({
              next: result => this.onSaveComplete(result),
              error: err => console.error(err)
            })
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
    if (result) {
      this.personalTrans.id = result.id;
    }
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
          next: result => this.loadTrans(result),
          error: err => console.error(err)
        });
    }
  }

  loadTrans(result: IPersonalTransaction) {
    this.personalTrans = result;
    this.personalTransForm.patchValue(this.personalTrans);
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
