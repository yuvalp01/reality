import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatSnackBar, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { IInsurance } from 'src/app/models';
import { InsuranceFormComponent } from '../insurance-form/insurance-form.component';
import { InsuranceService } from '../insurance.service';

@Component({
  selector: 'app-insurance-list',
  templateUrl: './insurance-list.component.html',
  styleUrls: ['./insurance-list.component.css']
})
export class InsuranceListComponent implements OnInit {
  constructor(private insuranceService: InsuranceService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
  ) { }
  dataSource = new MatTableDataSource<IInsurance>();
  insurances: IInsurance[];
  displayedColumns: string[] = [
    'id',
    'apartment',
    'company',
    'price',
    'dateStart',
    'dateEnd',
    'actions'
  ]

  ngOnInit() {
    this.loadItems();
  }

  loadItems() {
    this.insuranceService.getAll().subscribe({
      next: (result) => { this.dataSource.data = result },
      error: (err) => console.error(err)
    })
  }

  openForm(item: IInsurance) {
    let dialogLocal = this.dialog.open(InsuranceFormComponent, {
      height: 'auto',
      width: 'auto',
      data: item
    });
    dialogLocal.afterClosed().subscribe(() => this.loadItems())
  }

  delete(id: number) {
    if (confirm("Are you sure you want to delete?")) {
      this.insuranceService.delete(id).subscribe({
        next: () => {
          this.snackBar.open(`Insurance id ${id}`, 'Deleted', { duration: 2000 });
          this.loadItems();
        },
        error: (err) => console.error(err)
      })
    }
  }

}
