import { Component, OnInit } from '@angular/core';
import { IRenovationProduct } from 'src/app/models';
import { MatTableDataSource, MatSnackBar, MatDialog } from '@angular/material';
import { RenovationService } from 'src/app/services/renovation.service';
import { ProductFormComponent } from '../product-form/product-form.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  dataSource = new MatTableDataSource<IRenovationProduct>();
  displayedColumns: string[] = ['id', 'name', 'description', 'store', 'price', 'photoUrl', 'link', 'serialNumber', 'actions'];

  constructor(private renovationService: RenovationService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loadList()

  }

  loadList() {
    this.renovationService.getProducts().subscribe({
      next: (result) => {
        this.dataSource.data = result;
      },
      error: (err) => console.error(err)
    });
  }
  openForm(itemId: number) {
    let dialogLocal = this.dialog.open(ProductFormComponent, {
      height: 'auto',
      width: '500px',
      data: itemId
    });
    dialogLocal.componentInstance.refreshEmitter.subscribe(() => this.loadList())
  }

  delete(id: number) {
    if (confirm("Are you sure you want to delete?"))
    {
      this.renovationService.deleteProduct(id).subscribe({
        next: () => this.loadList(),
        error: (err) => console.error(err)
      });
    }
  }
}

