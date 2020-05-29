import { Component, OnInit, ViewChild, Input, SimpleChanges, OnChanges } from '@angular/core';
import { IPersonalTransaction } from '../../models';
import { PersonalTransService } from '../personal-trans.service';
import { MatTableDataSource, MatSort, MatDialog, fadeInContent } from '@angular/material';
import { PersonalTransFormComponent } from '.././personal-trans-form/personal-trans-form.component';
import { Router } from '@angular/router';
import { ExcelService } from '../../services/excel.service';


@Component({
  selector: 'app-personal-trans',
  templateUrl: './personal-trans.component.html',
  styleUrls: ['./personal-trans.component.css']
})
export class PersonalTransComponent implements OnChanges, OnInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(
    private route: Router,
    private excelService: ExcelService,
    private personalTransService: PersonalTransService,
    private dialog: MatDialog) { }
  displayedColumns: string[] = ['date', 'amount', 'transactionType', 'apartment', 'comments'];
  balance: number = 0;

  dataSourceTrans = new MatTableDataSource<IPersonalTransaction>();
  @Input() editable: boolean = true;
  @Input() stakeholderId: number;

  show_paidOnBehalf: boolean = true;
  show_profitDistribution: boolean = true;
  show_cashWithdrawal: boolean = true;
  show_moneyTransfer: boolean = true;


  ngOnInit() {
    if (this.editable) {
      //      this.displayedColumns.push('actions');
      this.displayedColumns = ['date', 'stakeholderId', 'amount', 'transactionType', 'apartment', 'comments', 'actions'];
    }
    //this.refreshData(this.stakeholderId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.stakeholderId = changes.stakeholderId.currentValue;
    this.refreshData(this.stakeholderId);
  }


  refreshData(stakeholderId: number) {
    //refresh tables
    this.personalTransService.getPesonalTransByStakeholder(stakeholderId).subscribe(result => {
      let personalTrans = result as IPersonalTransaction[];
      this.dataSourceTrans.data = personalTrans;
      this.dataSourceTrans.sort = this.sort;
      this.dataSourceTrans.filterPredicate = (data: any, filter: string):boolean => {
        return data.transactionType == filter;
      };

    }, error => console.error(error));
    //Refresh balance
    this.personalTransService.getPersonalBalance(stakeholderId).subscribe(result => this.balance = result, error => console.error(error));
  }


  ngAfterViewInit(): void {
    this.dataSourceTrans.sort = this.sort;
  }

  openForm(_transactionId: number) {
    let dialogRef = this.dialog.open(PersonalTransFormComponent, {
      height: '600px',
      width: '500px',
      data: { transactionId: _transactionId }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe(() => this.refreshData(this.stakeholderId));
    dialogRef.componentInstance.chageStakeholderEmitter.subscribe(id => {
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

  switchFilter(type) {
    //var id = event.target.attributes.id.nodeValue;
    switch (type) {
      case 'paidOnBehalf':
        this.applyFilter('10');
        this.show_paidOnBehalf = true;
        //turn off other filters
        this.show_profitDistribution = false;
        this.show_cashWithdrawal = false;
        this.show_moneyTransfer = false;
        break;
      case 'profitDistribution':
        this.applyFilter('20');
        this.show_profitDistribution = true;
        //turn off other filters
        this.show_paidOnBehalf = false;
        this.show_cashWithdrawal = false;
        this.show_moneyTransfer = false;
        break;
      case 'cashWithdrawal':
        this.applyFilter('13');
        this.show_cashWithdrawal = true;
        //turn off other filters
        this.show_paidOnBehalf = false;
        this.show_profitDistribution = false;
        this.show_moneyTransfer = false;
        break;
      case 'moneyTransfer':
        this.applyFilter('5');
        this.show_moneyTransfer = true;
        //turn off other filters
        this.show_paidOnBehalf = false;
        this.show_profitDistribution = false;
        this.show_cashWithdrawal = false;

        break;
      default:
        this.applyFilter('');
        this.show_moneyTransfer = true;
        this.show_cashWithdrawal = true;
        this.show_paidOnBehalf = true;
        this.show_profitDistribution = true;
        break;

    }
  }

  applyFilter(filterValue: string) {
    // filterValue = filterValue.trim(); // Remove whitespace
    // filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSourceTrans.filter = filterValue;
  }

  //filterByString(data, s) {
  //  return data.filter(e => e.transactionType==s);
  //  //.sort((a, b) => a.id.includes(s) && !b.id.includes(s) ? -1 : b.id.includes(s) && !a.id.includes(s) ? 1 : 0);
  //}


  public isPositive(value: number): boolean {
    if (value >= 0) {
      return true;
    }
    return false;
  }

  exportAsXLSX(): void {

    this.dataSourceTrans.data.forEach(a => delete a.id);
    this.dataSourceTrans.data.forEach(a => delete a.stakeholderId);

    //this.dataSourceTrans.data.forEach(a => a.date = a.date.toDateString())
    //this.dataSourceTrans.data.forEach(a => a.date = new Date(a.date).toLocaleDateString('en-GB')) as const;
    //this.dataSourceTrans.data.forEach(a => a.date3 = new Date(a.date).toDateString('yyyy-MM-dd'));
    //this.dataSourceTrans.data.forEach(a => {
    //  let d = new Date(a.date);
    //  //let ds = `${d.getFullYear()}-${d.getMonth() + 1}-${d.getDate()}`;
    //  a['Date'] = `${d.getFullYear()}-${d.getMonth() + 1}-${d.getDate()}`;
    //  delete a.date;
    //});
    //debugger;

    this.excelService.exportAsExcelFile(this.dataSourceTrans.data, 'Transactions');
  }
}
