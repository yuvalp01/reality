import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { IssuesService } from '../issues.service';
import { IIssue } from '../../models/issues';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { IApartment } from 'src/app/models';
import { ApartmentService } from 'src/app/services/apartment.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';


@Component({
  selector: 'issue-form',
  templateUrl: './issue-form.component.html',
  styleUrls: ['./issue-form.component.css']
})
export class IssueFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<IssueFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private issueService: IssuesService,
    private apartmentService: ApartmentService) { }

  issueForm: FormGroup;
 // issueId: number = 1;
  apartments: IApartment[];
  @Output() refreshEmitter = new EventEmitter();

  ngOnInit() {
    this.apartmentService.getApartments().subscribe({
      next: result => this.apartments = result,
      error: err => console.error(err)
    });
    this.issueForm = this.formBuilder.group({
      id: [0],
      title: [null, Validators.required],
      description: null,
      priority: [3, Validators.compose([Validators.min(1), Validators.max(3)])],
      apartmentId: 0,
      dateOpen: [new Date(), Validators.required],
      dateClose: null,
      isNew:[true]
    });
    
    if (this.data) {
      this.loadItem(this.data as IIssue);
    }
  }
  loadItem(item: IIssue) {
    this.issueForm.patchValue(item);
  }

  save() {
    if (this.issueForm.valid) {
      if (this.issueForm.dirty) {
        // const t: ITransaction = { ...this.transaction, ...this.transactionFormGroup.value }
        var issue:IIssue = Object.assign({}, this.issueForm.value);
        issue.dateOpen = this.fixUtcDate(issue.dateOpen);
        issue.dateClose = this.fixUtcDate(issue.dateClose);
       // issue.isNew = true;
        if (this.data) {
          this.issueService.updateIssue(issue).subscribe({
            next: () => this.afterSave('Updated'),
            error: err => console.error(err)
          })
        }
        else{
          this.issueService.addNewIssue(issue).subscribe({
            next: () => this.afterSave('Added'),
            error: err => console.error(err)
          })
        }
      }
    }
  }
  afterSave(action:string) {
    this.dialogRef.close();
    let snackBarRef = this.snackBar.open(`Issue`, action, { duration: 2000 });
    this.refreshEmitter.emit();
  }

  
  public fixUtcDate(dateIn) {
    if(!dateIn)return null;
    ///fix UTC issue:
    let date = new Date(dateIn);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 3);
    ///
  }
}


