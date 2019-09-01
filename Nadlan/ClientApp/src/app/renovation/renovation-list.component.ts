import { Component, OnInit } from '@angular/core';
import { ExcelService } from '../services/excel.service';
import { ILine, IItemDto } from '../models';
import { RenovationService } from '../services/renovation.service';


@Component({
  templateUrl: './renovation-list.component.html',
})
export class RenovationListComponent implements OnInit {
  constructor(private renovationService: RenovationService, private excelService:ExcelService) { }
  generalLines: ILine[];
  kitchenLines: ILine[];
  bathLines: ILine[];
  allItems: IItemDto[];
  //data: ILine[];

  ngOnInit() {
    this.renovationService.getRenovationLines(6).subscribe(result => {
      //this.renovationLines = result;
      this.generalLines = result.filter(a => a.category == 0);
      this.kitchenLines = result.filter(a => a.category == 1);
      this.bathLines = result.filter(a => a.category == 2);
      //this.dataSource.data = result as ILine[];
    }, error => console.error(error));

    this.renovationService.getRenovationItems(-1).subscribe(result => {
      this.allItems = result;
    }, error => console.error(error));

  }
  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.allItems, 'sample');
  }
}








    //this.dataSource.filterPredicate = function (data, filter: any): boolean {
    //  return data.category == filter;//apartmentAddress.toLowerCase().includes(filter);
    //};
    //this.dataSource.filter =  "0";

    //this.dataSource.filterPredicate = function (data, filter: string): boolean {
    //  return data.apartmentAddress.toLowerCase().includes(filter);
    //};


    //this.dataSource.filter = 
