import { Component, OnInit, ViewChild } from '@angular/core';
import { RenovationService } from '../../services/renovation.service';
import { TransactionService } from '../../services/transaction.service';
import { IRenovationLine, IRenovationProduct, IRenovationProject } from '../../models';
import { MatTableDataSource, MatDialog, MatSort, MatSortable } from '@angular/material';
import { IRenovationPayment, ITransaction } from '../../models';
import { PaymentFormComponent } from '../payment-form/payment-form.component';
import { ExcelService } from '../../services/excel.service';
import { ActivatedRoute } from '@angular/router';
import { SecurityService } from 'src/app/security/security.service';

@Component({
  selector: 'app-renovation-overview',
  templateUrl: './renovation-overview.component.html',
  styleUrls: ['./renovation-overview.component.css']
})
export class RenovationOverviewComponent implements OnInit {

  @ViewChild(MatSort, { static: false }) sort: MatSort;
  constructor(private renovationService: RenovationService,
    private transactionService: TransactionService,
    private securityService: SecurityService,
    private excelService: ExcelService,
    private dialog: MatDialog,
    private route: ActivatedRoute) {
  }
  dataSourceLines = new MatTableDataSource<IRenovationLine>();
  dataSourcePayments = new MatTableDataSource<IRenovationPayment>();
  displayedColumns_lines: string[] = ['itemType', 'title', 'units', 'cost', 'comments', 'isCompleted','actions'];
  displayedColumns_lines_filtered:string[];
  displayedColumns_payments: string[] = ['title', 'criteria', 'amount', 'comments', 'datePayment', 'checkIdWriten', 'checkInvoiceScanned', 'checkCriteriaMet', 'actions'];


  project: IRenovationProject;
  products: IRenovationProduct[];
  filteredProducts: IRenovationProduct[];
  totalCost: number = 0;
  totalPaymentsPlanned: number = 0;
  totalPaymentsDone: number = 0;
  totalLinesDone: number = 0;
  paymentTransaction: ITransaction;
  transactionAmount: number = 0;
  getRenovationPayments: IRenovationPayment[];
  progressPercent: string = '0%';
  productType = 'work';
  projectId: number = 0;


  ngOnInit() {
    this.route.paramMap.subscribe(a => {
      this.projectId = +a.get('projectId')
      this.loadData(this.projectId);
    });
  }

  loadData(projectId: number) {

    this.loadProject(projectId);
    this.loadPayments(projectId, null);
    //this.loadLines(projectId);
  }

  // ngAfterViewInit() {
  //   this.dataSourceLines.sort = this.sort;
  // }

  loadLines(projectId: number) {
    this.renovationService.getRenovationLines(projectId)
      .subscribe(result => {
        let lines = result
        this.totalCost = lines.reduce((total, line) => total + line.cost, 0);
        // if(this.planningMode){
        //   this.sort.sort(({ id: 'id', start: 'desc'}) as MatSortable);
        // }
        // else{
        //   this.sort.sort(({ id: 'category', start: 'asc'}) as MatSortable);
        // }
        if (
          //  this.securityService.securityObject.claims.findIndex(a => a.claimType == 'admin') == -1 ||
          this.project.stage != 30) {
            this.displayedColumns_lines_filtered = this.displayedColumns_lines;
          // this.displayedColumns_lines.push('actions');
          }
          else{
            this.displayedColumns_lines_filtered = this.displayedColumns_lines.filter(a=>a!='actions');
            //this.displayedColumns_lines.splice(this.displayedColumns_lines.indexOf('actions'),)
          }
        this.dataSourceLines.sort = this.sort;
        this.dataSourceLines.data = lines;
        this.calcCompletedLines();
        if (this.products == undefined) {
          this.loadProducts();
        }
      }, error => console.error(error));
  }

  //   sortData(obj){
  // debugger;
  //   }
  filterType(type) {
    this.productType = type;
    this.filteredProducts = this.products.filter(a => a.itemType == this.productType);

  }

  loadProducts() {
    this.renovationService.getProducts().subscribe({
      next: (result) => this.afterProductsLoad(result),
      error: (err) => console.error(err)
    });
  }


  afterProductsLoad(result: IRenovationProduct[]) {
    this.products = result
    this.products.forEach(a => {
      // a.selectedItems = this.dataSourceLines.data.filter(l => l.productId == a.id).length;
      let lines = this.dataSourceLines.data.filter(l => l.productId == a.id);
      a.selectedItems = lines.reduce((total, line) => total + line.units, 0);

    })
    this.filterType('work');
  }

  loadProject(projectId: number) {
    this.renovationService.getRenovationProject(projectId)
      .subscribe(result => {
        this.project = result;
        this.loadTransaction(this.project.transactionId);
        this.loadLines(projectId);
      }, error => console.error(error));

  }

  loadTransaction(transactionId) {
    this.transactionService.getTransactionById(transactionId)
      .subscribe(result => {
        if (result) {
          this.paymentTransaction = result;
          this.transactionAmount = result.amount * -1;
        }

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
          this.transactionAmount = newBalance * -1;
        }
      }, error => console.error(error));
  }

  private calcCompletedLines() {
    let linesDone: IRenovationLine[] = this.dataSourceLines.data.filter(a => a.isCompleted);
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
      this.loadPayments(this.loadPayments, result);
      dialogRef.close();
    });

  }

  confirm(id: number) {
    this.renovationService.confirmPayment(id)
      .subscribe({
        next: () => this.loadPayments(this.projectId, null),
        error: err => console.error(err)
      });
  }
  delete(id: number) {
    if (confirm("Are you sure you want to delete?")) {
      this.renovationService.deletePayment(id)
        .subscribe({
          next: newBalance => this.loadPayments(this.projectId, newBalance),
          error: err => console.error(err)
        });
    }
  }

  deleteLine(line: IRenovationLine) {

    if (confirm("Are you sure you want to delete?")) {
      this.renovationService.deleteLine(line.id).subscribe({
        next: () => { this.loadLines(this.projectId); this.decreaseProductCount(line.productId) },
        error: (err) => console.error(err)
      });
    }
  }

  decreaseProductCount(productId: number) {
    let product = this.products.find(a => a.id == productId);
    product.selectedItems--;
  }


  saveProject(project: IRenovationProject) {
    // console.log(this.test.nativeElement.value);
    // this.test.nativeElement.value = "new val";


    this.renovationService.updateProject(project).subscribe({
      // next: (result) => {console.log(this.test.nativeElement)},
      next: (result) => { },
      error: (err) => { console.log(err); }
    });
  }

  handleInput(event: KeyboardEvent): void {
    event.stopPropagation();
  }


  planningMode: boolean;
  itemEditMode() {
    this.planningMode = !this.planningMode;
    this.sort.sort(({ id: 'id', start: 'desc' }) as MatSortable);
  }


  addItem(product: IRenovationProduct) {
    product.selectedItems++;
    let newLine: IRenovationLine = Object.assign({});
    newLine.productId = product.id;
    newLine.renovationProjectId = this.projectId;
    newLine.title = product.name;
    newLine.comments = product.description;
    newLine.units = 1;
    newLine.cost = product.price;
    newLine.insertStage = this.project.stage;

    this.renovationService.addLine(newLine).subscribe({
      next: () => this.loadLines(this.projectId),
      error: (err) => console.error(err)
    });
  }


  saveLine(line: IRenovationLine) {
    this.renovationService.updateLine(line).subscribe({
      next: (result) => this.afterSaveLine(line),
      error: (err) => console.log(err)
    });
  }

  afterSaveLine(line: IRenovationLine) {
    line.isEditMode = false;
    this.totalCost = this.dataSourceLines.data.reduce((total, line) => total + line.cost, 0);
  }



  cancelPayment(id: number) {
    this.renovationService.cancelPayment(id)
      .subscribe({
        next: newBalance => this.loadPayments(this.projectId, newBalance),
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
        next: () => this.loadLines(this.projectId),
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
