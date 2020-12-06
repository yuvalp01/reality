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
  displayedColumns: string[] = ['id', 'name', 'description','type', 'store', 'price', 'photoUrl', 'link', 'serialNumber', 'actions'];

  show_work: boolean = true;
  show_furniture: boolean = true;
  show_amenities: boolean = true;
  show_appliances: boolean = true;
  show_fixtures: boolean = true;

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
        this.dataSource.filterPredicate = (data: any, filter: string): boolean => {
          return data.itemType == filter;
        };
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

  switchFilter(type) {
    this.dataSource.filter =type;
    switch (type) {
      case 'work':
        this.show_work = true;
        //turn off other filters
        this.show_furniture = false;
        this.show_amenities = false;
        this.show_appliances = false;
        this.show_fixtures = false;
        break;
      case 'furniture':
        this.show_furniture = true;
        //turn off other filters
        this.show_work = false;
        this.show_amenities = false;
        this.show_appliances = false;
        this.show_fixtures = false;
        break;
      case 'amenities':
        this.show_amenities = true;
        //turn off other filters
        this.show_work = false;
        this.show_furniture = false;
        this.show_appliances = false;
        this.show_fixtures = false;
        break;
      case 'appliances':
        this.show_appliances = true;
        //turn off other filters
        this.show_work = false;
        this.show_furniture = false;
        this.show_amenities = false;
        this.show_fixtures = false;
        break;
        case 'fixtures':
          this.show_fixtures = true;
          //turn off other filters
          this.show_work = false;
          this.show_furniture = false;
          this.show_appliances = false;
          this.show_amenities = false;
          break;
      default:
        this.show_appliances = true;
        this.show_amenities = true;
        this.show_work = true;
        this.show_furniture = true;
        this.show_fixtures = true;
        break;

    }
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

