import { Component, OnInit } from '@angular/core';
import { ContractService } from '../contract.service';
import { MatTableDataSource, MatDialog, MatSnackBar } from '@angular/material';
import { IContract } from 'src/app/models';
import { ContractFormComponent } from '../contract-form/contract-form.component';

@Component({
  selector: 'app-contract-list',
  templateUrl: './contract-list.component.html',
  styleUrls: ['./contract-list.component.css']
})
export class ContractListComponent implements OnInit {

  constructor(private contractService: ContractService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog) { }
  dataSource = new MatTableDataSource<IContract>();
  displayedColumns: string[] = ['apartment', 'tenant', 'price', 'dateStart', 'dateEnd', 'isElectriciyChanged','actions']
  ngOnInit() {
    this.loadItems();
  }

  loadItems() {
    this.contractService.getAll().subscribe({
      next: (result) => this.dataSource.data = result,
      error: (err) => console.error(err)
    })
  }

  openForm(item: IContract) {
    let dialogLocal = this.dialog.open(ContractFormComponent, {
      height: 'auto',
      width: 'auto',
      data: item
    });
    dialogLocal.afterClosed().subscribe(() => this.loadItems())
  }
  delete(id: number) {
    if (confirm("Are you sure you want to delete?")) {
      this.contractService.delete(id).subscribe({
        next: () => {
          this.snackBar.open(`Contract id ${id}`, 'Deleted', { duration: 2000 });
          this.loadItems();
        },
        error: (err) => console.error(err)
      })
    }
  }
}
