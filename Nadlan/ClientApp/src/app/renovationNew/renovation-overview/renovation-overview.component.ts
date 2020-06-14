import { Component, OnInit } from '@angular/core';
import { RenovationService } from '../../services/renovation.service';
import { TransactionService } from '../../services/transaction.service';
import { IRenovationLine, IRenovationProject } from '../../models';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { IRenovationPayment, ITransaction } from '../../models';
import { PaymentFormComponent } from '../payment-form/payment-form.component';
import { CloseScrollStrategy, ScrollStrategyOptions } from '@angular/cdk/overlay';

@Component({
  selector: 'app-renovation-overview',
  templateUrl: './renovation-overview.component.html',
  styleUrls: ['./renovation-overview.component.css']
})
export class RenovationOverviewComponent implements OnInit {

  constructor(private renovationService: RenovationService,
    private transactionService: TransactionService,
    private dialog: MatDialog) { }
  dataSourceLines = new MatTableDataSource<IRenovationLine>();
  dataSourcePayments = new MatTableDataSource<IRenovationPayment>();
  displayedColumns_lines: string[] = ['category', 'id', 'title', 'cost', 'comments', 'isCompleted'];
  displayedColumns_payments: string[] = ['id', 'title', 'criteria', 'amount', 'comments', 'datePayment', 'checkIdWriten', 'checkInvoiceScanned', 'checkCriteriaMet', 'actions'];

  project: IRenovationProject;
  // lines: IRenovationLine[];
  totalCost: number = 0;
  totalPaymentsPlanned: number = 0;
  totalPaymentsDone: number = 0;
  paymentTransaction: ITransaction;
  getRenovationPayments: IRenovationPayment[];
  progressPercent:string = '33%';
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
      });
  }


  loadLines() {
    this.renovationService.getRenovationLinesNew(this.PROJECT_ID)
      .subscribe(result => {
        let lines = result
        this.totalCost = lines.reduce((total, line) => total + line.cost, 0); 
        this.dataSourceLines.data = lines;
      }, error => console.error(error));

  }

  loadPayments(projectId, newBalance: number) {
    this.renovationService.getRenovationPayments(projectId)
      .subscribe(result => {
        this.dataSourcePayments.data = result;
        this.totalPaymentsPlanned = result.reduce((total,payment)=>total+payment.amount,0);
        var alreadyPaid = result.filter(a => a.datePayment);
        this.totalPaymentsDone = alreadyPaid.reduce((total, payment) => total + payment.amount, 0);
        if (newBalance) {
          this.paymentTransaction.amount = newBalance;
        }
      }, error => console.error(error));
  }

  openForm(_actionType: string, _renovationProjectId: number) {
    let dialogRef = this.dialog.open(PaymentFormComponent, {
      maxHeight: '580px',
      // panelClass: 'xxx',
      width: '500px',
      data: {
        renovationProjectId: _renovationProjectId,
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
          next: () => this.loadPayments(this.PROJECT_ID, null),
          error: err => console.error(err)
        });
    }
  }

  printId(id) {
    console.log(id);
  }

}
