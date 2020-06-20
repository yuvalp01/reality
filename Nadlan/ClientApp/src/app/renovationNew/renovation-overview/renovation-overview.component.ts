import { Component, OnInit } from '@angular/core';
import { RenovationService } from '../../services/renovation.service';
import { TransactionService } from '../../services/transaction.service';
import { IRenovationLine, IRenovationProject } from '../../models';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { IRenovationPayment, ITransaction } from '../../models';
import { PaymentFormComponent } from '../payment-form/payment-form.component';
import { ExcelService } from '../../services/excel.service';

@Component({
  selector: 'app-renovation-overview',
  templateUrl: './renovation-overview.component.html',
  styleUrls: ['./renovation-overview.component.css']
})
export class RenovationOverviewComponent implements OnInit {

  constructor(private renovationService: RenovationService,
    private transactionService: TransactionService,
    private excelService: ExcelService,
    private dialog: MatDialog) { }
  dataSourceLines = new MatTableDataSource<IRenovationLine>();
  dataSourcePayments = new MatTableDataSource<IRenovationPayment>();
  displayedColumns_lines: string[] = ['category', 'title', 'cost', 'comments', 'isCompleted'];
  displayedColumns_payments: string[] = ['title', 'criteria', 'amount', 'comments', 'datePayment', 'checkIdWriten', 'checkInvoiceScanned', 'checkCriteriaMet', 'actions'];

  project: IRenovationProject;
  // lines: IRenovationLine[];
  totalCost: number = 0;
  totalPaymentsPlanned: number = 0;
  totalPaymentsDone: number = 0;
  totalLinesDone: number = 0;
  paymentTransaction: ITransaction;
  transactionAmount: number = 0;
  getRenovationPayments: IRenovationPayment[];
  progressPercent: string = '0%';
  readonly PROJECT_ID: number = 1;
  ngOnInit() {
    this.loadData();
  }

  loadData() {

    this.loadProjects();
    this.loadLines();
    this.loadPayments(this.PROJECT_ID, null);

  }

  loadProjects() {
    this.renovationService.getRenovationProjects()
      .subscribe(result => {
        this.project = result[0];
        this.loadTransaction(this.project.transactionId);
      }, error => console.error(error));

  }

  loadTransaction(transactionId) {
    this.transactionService.getTransactionById(transactionId)
      .subscribe(result => {
        this.paymentTransaction = result;
        this.transactionAmount = result.amount*-1;
      });
  }


  loadLines() {
    this.renovationService.getRenovationLinesNew(this.PROJECT_ID)
      .subscribe(result => {
        let lines = result
        this.totalCost = lines.reduce((total, line) => total + line.cost, 0);
        this.dataSourceLines.data = lines;
        this.calcCompletedLines();
      }, error => console.error(error));

  }

  loadPayments(projectId, newBalance: number) {
    this.renovationService.getRenovationPayments(projectId)
      .subscribe(result => {
        this.dataSourcePayments.data = result;
        this.totalPaymentsPlanned = result.reduce((total, payment) => total + payment.amount, 0);
        var alreadyPaid = result.filter(a => a.datePayment);
        this.totalPaymentsDone = alreadyPaid.reduce((total, payment) => total + payment.amount, 0);
        if (newBalance) {
          this.transactionAmount = newBalance*-1;
        }
      }, error => console.error(error));
  }

  private calcCompletedLines() {
    let linesDone: IRenovationLine[] = this.dataSourceLines.data.filter(a=>a.isCompleted);
    this.totalLinesDone = linesDone.reduce((total, line) => total + line.cost, 0);
  }


  openForm(_actionType: string, _paymentId: number) {
    let dialogRef = this.dialog.open(PaymentFormComponent, {
      maxHeight: '580px',
      // panelClass: 'xxx',
      width: '500px',
      data: {
        paymentId: _paymentId,
        actionType: _actionType,
        //  isEdit : _isEdit
      }
    });
    dialogRef.componentInstance.refreshEmitter.subscribe((result) => {
      this.loadPayments(1, result);
      dialogRef.close();
    });

  }

  confirm(id: number) {
    this.renovationService.confirmPayment(id)
      .subscribe({
        next: () => this.loadPayments(this.PROJECT_ID, null),
        error: err => console.error(err)
      });
  }
  delete(id: number) {
    if (confirm("Are you sure you want to delete?")) {
      this.renovationService.deletePayment(id)
        .subscribe({
          next: newBalance => this.loadPayments(this.PROJECT_ID, newBalance),
          error: err => console.error(err)
        });
    }
  }
  cancelPayment(id:number)
  {
    this.renovationService.cancelPayment(id)
    .subscribe({
      next: newBalance => this.loadPayments(this.PROJECT_ID, newBalance),
      error: err => console.error(err)
    });
  }

  exportAsXLSX(): void {
    this.dataSourceLines.data.forEach(a => delete a.id);
    this.dataSourceLines.data.forEach(a => delete a.renovationProject);
    this.dataSourceLines.data.forEach(a => delete a['renovationProjectId']);
    this.dataSourceLines.data.forEach(a => {
      a['Category'] = this.getCategoryName(a.category);
      delete a.category;
    });
    this.excelService.exportAsExcelFile(this.dataSourceLines.data, 'Renovaiton - ' + this.project.name);
  }


  complete(event: any, item: IRenovationLine) {
    item.isCompleted = event.checked;
    this.renovationService.updateLine(item).subscribe(
      {
        next: () => this.loadLines(),
        error: err => console.error(err)
      });
  }



  printId(id) {
    console.log(id);
  }

  getCategoryName(num: number) {
    switch (num) {
      case 0:
        return 'General';
      case 1:
        return 'Kitchen';
      case 2:
        return 'Bathroom';
      case 3:
        return 'Rooms'
    }

  }

}
