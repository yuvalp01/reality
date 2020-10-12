import { Component, OnInit, NgZone, Inject, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MAT_DIALOG_DATA } from '@angular/material';
import { RenovationService } from '../../services/renovation.service';
import { IRenovationPayment } from '../../models';
import { debug } from 'util';
import { UtilitiesService } from '../../services/utilities.service';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { SecurityService } from '../../security/security.service';

@Component({
  selector: 'app-payment-form',
  templateUrl: './payment-form.component.html',
  styleUrls: ['./payment-form.component.css']
})
export class PaymentFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private renovationService: RenovationService,
    private securityService: SecurityService,
    private utilitiesService: UtilitiesService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  formTitle: string;
  formIcon: string = 'monetization_on';
  renovationPaymentForm: FormGroup;
  renovationPayment: IRenovationPayment;
  clicked: boolean = false;

  @Output() refreshEmitter = new EventEmitter();

  ngOnInit() {

    this.renovationPaymentForm = this.formBuilder.group({
      renovationProjectId: [0, Validators.required],
      title: ['', Validators.required],
      amount: [0, Validators.required],
      criteria: ['', Validators.required],
      comments: [null],
      datePayment: [null],
      checkIdWriten: [false],
      checkInvoiceScanned: [false],
      checkCriteriaMet: [false],
      isConfirmed: [false],
    });
    this.loadData();

  }
  loadData() {
    let isAdmin = this.securityService.hasClaim('admin');

    //add new
    if (this.data.paymentId == 0) {
      this.formTitle = "Add new";
      this.renovationPaymentForm.controls.title.enable();
      //  this.isLimitedMode = false;
    }
    //for edit - load the data
    else {
      if (!isAdmin) {
        //this.renovationPaymentForm.controls.title.disable();
        // this.renovationPaymentForm.controls.amount.disable();
        this.formTitle = 'Make a payment for:'
        this.renovationPaymentForm.controls.comments.setValidators(Validators.required);
        this.renovationPaymentForm.controls.datePayment.setValidators(Validators.required);
        this.renovationPaymentForm.controls.checkIdWriten.setValidators(Validators.requiredTrue);
        this.renovationPaymentForm.controls.checkInvoiceScanned.setValidators(Validators.requiredTrue);
        this.renovationPaymentForm.controls.checkCriteriaMet.setValidators(Validators.requiredTrue);

      }
      else {
        this.formTitle = "Edit Payment";

      }

      //this.loadTrans(this.data.transactionId);
      this.renovationService.getRenovationPaymentById(this.data.paymentId)
        .subscribe({
          next: result => {
            this.loadItem(result)
          },
          error: err => console.error(err)
        });
    }
  }

  loadItem(result: IRenovationPayment) {
    this.renovationPayment = result;
    this.renovationPaymentForm.patchValue(this.renovationPayment);
    if (this.renovationPaymentForm.controls.datePayment.value) {
      this.renovationPaymentForm.controls.checkCriteriaMet.patchValue(true);
      this.renovationPaymentForm.controls.checkIdWriten.patchValue(true);
      this.renovationPaymentForm.controls.checkInvoiceScanned.patchValue(true);
      this.formTitle = 'Edit a payment for:'
      this.formIcon = 'edit';
    }
    //this.formTitle = 'Make payment for: ' + this.renovationPaymentForm.controls.title.value;
    //this.renovationPaymentForm.get('stakeholderId').disable();;
    //let stakeholderIdControl =
    //stakeholderIdControl.disable();
  }


  save() {
    if (this.renovationPaymentForm.valid) {
      if (this.renovationPaymentForm.dirty) {
        const t: IRenovationPayment = { ...this.renovationPayment, ...this.renovationPaymentForm.value }
        if (t.datePayment) {
          t.datePayment = this.utilitiesService.fixUtcDate(t.datePayment);
        }
        this.clicked = true;
        switch (this.data.actionType) {
          case 'add':
            this.renovationService.addRenovationPayment(t)
              .subscribe({
                next: result => this.onSaveComplete('Added', null),
                error: err => console.error(err)
              });
            break;
          case 'edit':
            this.renovationService.updateRenovationPayment(t)
              .subscribe({
                next: result => this.onSaveComplete('Updated', null),
                error: err => console.error(err)
              });
            break;
          case 'payment':
            this.renovationService.makePayment(t)
              .subscribe({
                next: result => this.onSaveComplete('Paid', result),
                error: err => console.error(err)
              });
            break;

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


  // onSaveComplete(result: IRenovationPayment) {
  onSaveComplete(_action: string, result: number) {

    if (result) {
      _action = _action + '. New balance: ' + result;

    }

    //let action = 'Updated';
    // if (result) {
    //   this.renovationPayment = result;
    //   if(result instanceof IRenovationPayment)
    //   {

    //     action = 'Added';
    //   }
    //   else
    //   {
    //     action = 'Paid';
    //   }

    //}
    let snackBarRef = this.snackBar.open(`Renovation Payment`, _action, { duration: 2000 });
    this.refreshEmitter.emit(result);
  }



  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
