import { Component, OnInit, ViewChild, NgZone, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IStakeholder, IPersonalTransaction } from '../models';
import { PersonalTransComponent } from './personal-trans.component';
import { PersonalTransService } from '../services/personal-trans.service';
import { MAT_DIALOG_DATA } from '@angular/material';
import { element } from 'protractor';

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
    console.log(this.data.transactionId);
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
    }

  }


  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
