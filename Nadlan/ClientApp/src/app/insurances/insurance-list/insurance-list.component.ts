import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { IInsurance } from 'src/app/models';
import { InsuranceService } from '../insurance.service';

@Component({
  selector: 'app-insurance-list',
  templateUrl: './insurance-list.component.html',
  styleUrls: ['./insurance-list.component.css']
})
export class InsuranceListComponent implements OnInit {

  constructor(private insuranceService: InsuranceService) { }
  dataSource = new MatTableDataSource<IInsurance>();
  insurances: IInsurance[];
  displayedColumns: string[] = [
    'id',
    'apartment',
    'company',
    'price',
    'dateStart',
    'dateEnd',
    // 'actions'
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

}
