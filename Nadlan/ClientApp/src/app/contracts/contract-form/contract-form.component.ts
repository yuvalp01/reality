import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { ContractService } from '../contract.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApartmentService } from 'src/app/services/apartment.service';
import { IApartment, IBankAccount, IContract } from 'src/app/models';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { UtilitiesService } from 'src/app/services/utilities.service';
import { BankAccountService } from 'src/app/services/bankAaccount.service';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html',
  styleUrls: ['./contract-form.component.css']
})
export class ContractFormComponent implements OnInit {

  constructor(private contractService: ContractService,
    private apartmentService: ApartmentService,
    private utilitiesService: UtilitiesService,
    private bankAccountService: BankAccountService,
    public dialogRef: MatDialogRef<ContractFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder) { }
  contractForm: FormGroup;
  apartments: IApartment[];
  bankAccounts: IBankAccount[];
  @Output() refreshEmitter = new EventEmitter();
  ngOnInit() {
    this.apartmentService.getApartments().subscribe({
      next: result => this.apartments = result,
      error: err => console.error(err)
    });
    this.contractForm = this.formBuilder.group({
      id: 0,
      apartmentId: [null, Validators.required],
      tenant: [null, Validators.required],
      tenantPhone: null,
      tenantEmail: null,
      dateStart: [null],
      dateEnd: [null],
      paymentDay: [1, [Validators.min(1), Validators.max(31)]],
      price: [null, Validators.required],
      penaltyPerDay: [0],
      deposit: [0],
      link: [null],
      isElectriciyChanged: [false],
      conditions: [null],
      bankAccountId: [-1, Validators.min(0)],

    });
    if (this.data) {
      this.loadItem(this.data as IContract);
    }
  }
  loadItem(item: IContract) {
    this.contractForm.patchValue(item);
    this.bankAccountService.getBankAccounts().subscribe(result => {
      this.bankAccounts = result;
    }, error => console.error(error));
  }

  save() {
    if (this.contractForm.valid) {
      if (this.contractForm.dirty) {
        var contract: IContract = Object.assign({}, this.contractForm.value);
        contract.dateStart = this.utilitiesService.fixUtcDate(contract.dateStart);
        contract.dateEnd = this.utilitiesService.fixUtcDate(contract.dateEnd);
        if (this.data) {
          this.contractService.update(contract).subscribe({
            next: () => this.afterSave('Updated'),
            error: err => console.error(err)
          })
        }
        else {
          this.contractService.add(contract).subscribe({
            next: () => this.afterSave('Added'),
            error: err => console.error(err)
          })
        }
      }
    }
  }
  afterSave(action: string) {
    this.dialogRef.close();
    let snackBarRef = this.snackBar.open(`Contract`, action, { duration: 2000 });
    this.refreshEmitter.emit();
  }
}
