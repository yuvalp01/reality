import { Component, OnInit } from '@angular/core';
import { ILine, IItem } from '../models';
import { MatTableDataSource } from '@angular/material';
import { RenovationService } from '../services/renovation.service';
import { debounce } from 'rxjs/operators';

@Component({
  selector: 'app-renovation',
  templateUrl: './renovation.component.html',
  styleUrls: ['./renovation.component.css']
})
export class RenovationComponent implements OnInit {
  constructor(private renovationService: RenovationService) { }
  displayedColumns: string[] = ['title', 'category', 'workCost', 'comments'];
  dataSource = new MatTableDataSource<ILine>();
  //renovationLines: ILine[];
  generalLines: ILine[];
  kitchenLines: ILine[];
  bathLines: ILine[];

  ngOnInit() {
    this.renovationService.getRenovationLines(6).subscribe(result => {
      //this.renovationLines = result;
      this.generalLines = result.filter(a => a.category == 0);
      this.kitchenLines = result.filter(a => a.category == 1);
      this.bathLines = result.filter(a => a.category == 2);
      this.dataSource.data = result as ILine[];


    }, error => console.error(error));
    //this.dataSource.filterPredicate = function (data, filter: any): boolean {
    //  return data.category == filter;//apartmentAddress.toLowerCase().includes(filter);
    //};
    //this.dataSource.filter =  "0";

    //this.dataSource.filterPredicate = function (data, filter: string): boolean {
    //  return data.apartmentAddress.toLowerCase().includes(filter);
    //};


    //this.dataSource.filter = 
  }

}
