import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { InsuranceService } from '../insurance.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApartmentService } from 'src/app/services/apartment.service';
import { IApartment, IInsurance } from 'src/app/models';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { UtilitiesService } from 'src/app/services/utilities.service';


@Component({
  selector: 'app-insurance-form',
  templateUrl: './insurance-form.component.html',
  styleUrls: ['./insurance-form.component.css']
})
export class InsuranceFormComponent implements OnInit {

  constructor(private insuranceService: InsuranceService,
    private apartmentService: ApartmentService,
    private utilitiesService: UtilitiesService,
    public dialogRef: MatDialogRef<InsuranceFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder) { }
  insuranceForm: FormGroup;
  apartments: IApartment[];
  // bankAccounts: IBankAccount[];
  @Output() refreshEmitter = new EventEmitter();
  ngOnInit() {
    this.apartmentService.getApartments().subscribe({
      next: result => this.apartments = result,
      error: err => console.error(err)
    });

    this.insuranceForm = this.formBuilder.group({
      id: 0,
      apartmentId: [null, Validators.required],
      company: [null, Validators.required],
      dateStart: [null],
      dateEnd: [null],
      price: [null, Validators.required],


    });
    if (this.data) {
      this.loadItem(this.data as IInsurance);
    }
  }
  loadItem(item: IInsurance) {
    this.insuranceForm.patchValue(item);
  }

  save() {
    if (this.insuranceForm.valid) {
      if (this.insuranceForm.dirty) {
        var insurance: IInsurance = Object.assign({}, this.insuranceForm.value);
        insurance.dateStart = this.utilitiesService.fixUtcDate(insurance.dateStart);
        insurance.dateEnd = this.utilitiesService.fixUtcDate(insurance.dateEnd);
        if (this.data) {
          this.insuranceService.update(insurance, insurance.id).subscribe({
            next: () => this.afterSave('Updated'),
            error: err => console.error(err)
          })
        }
        else {
          this.insuranceService.add(insurance).subscribe({
            next: () => this.afterSave('Added'),
            error: err => console.error(err)
          })
        }
      }
    }
  }
  afterSave(action: string) {
    this.dialogRef.close();
    let snackBarRef = this.snackBar.open(`Insurance`, action, { duration: 2000 });
    this.refreshEmitter.emit();
  }
}
