import { Component, OnInit, ViewChild, Input, SimpleChanges, OnChanges } from '@angular/core';
import { ReportService } from '../../services/reports.service';
import { IPersonalTransaction } from '../../models';
import { PersonalTransService } from '../personal-trans.service';
import { MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { PersonalTransFormComponent } from '.././personal-trans-form/personal-trans-form.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-personal-trans',
  templateUrl: './personal-trans.component.html',
  styleUrls: ['./personal-trans.component.css']
})
export class PersonalTransComponent implements OnInit, OnChanges  {



  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(
    private route: Router,
    private personalTransService: PersonalTransService,
    private dialog: MatDialog) { }
  displayedColumns: string[] = ['date', 'amount','apartment', 'comments'];
  balance: number = 0;

  dataSourceTrans = new MatTableDataSource<IPersonalTransaction>();
  @Input() editable: boolean = true;
  @Input() stakeholderId: number;
  ngOnInit() {
    if (this.editable) {
//      this.displayedColumns.push('actions');
      this.displayedColumns = ['date', 'stakeholderId', 'amount','apartment', 'comments', 'actions'];
    }
    this.refreshData(this.stakeholderId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.stakeholderId = changes.stakeholderId.currentValue;
    this.refreshData(this.stakeholderId);
  }


  refreshData(stakeholderId:number) {
    //refresh tables
    this.personalTransService.getPesonalTransByStakeholder(stakeholderId).subscribe(result => {
      let personalTrans = result as IPersonalTransaction[];
      this.dataSourceTrans.data = personalTrans;
      this.dataSourceTrans.sort = this.sort;

    }, error => console.error(error));
    //Refresh balance
    this.personalTransService.getPersonalBalance(stakeholderId).subscribe(result => this.balance = result, error => console.error(error));
  }


  ngAfterViewInit(): void {
    this.dataSourceTrans.sort = this.sort;
  }

  openForm(_transactionId: number) {


    let dialogRef = this.dialog.open(PersonalTransFormComponent, {
      height: '500px',
      width: '500px',
      data: { transactionId: _transactionId }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData(this.stakeholderId));
    dialogRef.componentInstance.chageStakeholderEmitter.subscribe(id =>
    {
      this.route.navigate(['personal-admin', id]);
      //this.stakeholderId = id;
      //this.refreshData(id)
    });


  }
  delete(transactionId) {
    if (confirm("Are you sure you want to delete?")) {
      this.personalTransService.detelePersonalTrans(transactionId)
        .subscribe({
          next: () => this.onDeleteComplete(),
          error: err => console.error(err)
        });
    }
  }

  onDeleteComplete() {
    this.refreshData(this.stakeholderId);
  }



  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }


}
