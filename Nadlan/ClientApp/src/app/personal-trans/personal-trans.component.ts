import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ReportService } from '../services/reports.service';
import { IPersonalTransaction } from '../models';
import { PersonalTransService } from '../services/personal-trans.service';
import { MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { debounce } from 'rxjs/operators';
import { element } from 'protractor';
import { PersonalTransFormComponent } from './personal-trans-form.component';

@Component({
  selector: 'app-personal-trans',
  templateUrl: './personal-trans.component.html',
  styleUrls: ['./personal-trans.component.css']
})
export class PersonalTransComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(
    private reportService: ReportService,
    private personalTransService: PersonalTransService,
    private dialog: MatDialog) { }
  displayedColumns: string[] = ['date', 'stakeholderId', 'amount', 'comments'];

  balance: number = 0;
  dataSourceTrans = new MatTableDataSource<IPersonalTransaction>();
  @Input()
  editable: boolean = true;
  ngOnInit() {
    if (this.editable) {
      this.displayedColumns.push('actions');
    }
    this.refreshData();
  }

  refreshData() {
    //refresh tables
    this.personalTransService.getPesonalTransByStakeholder(2).subscribe(result => {
      let personalTrans = result as IPersonalTransaction[];
      this.dataSourceTrans.data = personalTrans;
      this.dataSourceTrans.sort = this.sort;

    }, error => console.error(error));
    //Refresh balance
    this.reportService.getPersonalBalance(1).subscribe(result => this.balance = result, error => console.error(error));
  }


  ngAfterViewInit(): void {
    this.dataSourceTrans.sort = this.sort;
  }

  

  openForm(_transactionId: number) {


    let dialogRef = this.dialog.open(PersonalTransFormComponent, {
      height: '500px',
      width: '500px',
      data: { transactionId: _transactionId}
      //data: { type: 'hours', visibleAccounts: [4, 6, 11] },
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData());
    dialogRef.afterClosed().subscribe(result => {
      this.refreshData();
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
    this.refreshData();
  }



  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }


}
