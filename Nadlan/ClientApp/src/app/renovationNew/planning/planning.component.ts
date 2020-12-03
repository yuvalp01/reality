import { Component, OnInit } from '@angular/core';
import { MatGridTileHeaderCssMatStyler, MatTableDataSource } from '@angular/material';
import { BehaviorSubject, Observable } from 'rxjs';
import { IRenovationLine, IRenovationProduct, IRenovationProject } from 'src/app/models';
import { RenovationService } from 'src/app/services/renovation.service';

@Component({
  selector: 'app-planning',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.css']
})
export class PlanningComponent implements OnInit {
  products: IRenovationProduct[] = [];
  lines: IRenovationLine[] = [];
  project: IRenovationProject;
  displayedColumns: string[] = [
    'title',
    'units',
    'cost',
    'comments',
    'actions'
  ];
  dataSource = new MatTableDataSource<IRenovationLine>();


  // selectedProducts: IRenovationProduct[] = [];


  constructor(private renovationService: RenovationService) { }

  ngOnInit() {
    this.loadItems();
    this.loadLines();
    this.loadProject(3);
  }

  loadProject(projectId: number) {
    this.renovationService.getRenovationProject(projectId).subscribe({
      next: (result) => this.project = result,
      error: (err) => console.error(err)
    });
  }

  saveLine(line: IRenovationLine) {
    console.log(line.id);
    this.renovationService.updateLine(line).subscribe({
      next: (result) => line.isEditMode = false,
      error: (err) => console.log(err)
    });
  }

  loadItems() {
    this.renovationService.getProductByType('work').subscribe({
      next: (result) => this.products = result,
      error: (err) => console.error(err)
    });
  }

  loadLines() {
    this.renovationService.getRenovationLines(3).subscribe({
      next: (result) => { this.lines = result; this.dataSource.data = result },
      error: (err) => console.error(err)
    });
  }

  filterType(type) {
    console.log(type);
    this.renovationService.getProductByType(type).subscribe({
      next: (result) => this.products = result,
      error: (err) => console.error(err)
    });
  }


  removeLine(itemId: number) {

    this.renovationService.deleteLine(itemId).subscribe({
      next: () => this.loadLines(),
      error: (err) => console.error(err)
    });

    // this.lines = this.lines.slice(this.lines.findIndex(a=>a.id==itemId),1);

  }


  addItem(product: IRenovationProduct) {

    let newLine: IRenovationLine = Object.assign({});
    // newLine.id = Math.max.apply(Math, this.lines.map(a => a.id)) + 1;
    newLine.renovationProjectId = 3;
    newLine.title = product.name;
    newLine.comments = product.description;
    newLine.units = 1;
    newLine.cost = product.price;

    // this.lines.push(newLine);

    this.renovationService.addLine(newLine).subscribe({
      next: () => this.loadLines(),
      error: (err) => console.error(err)
    });
  }

  getSum() {


    return this.lines.reduce((sum, current) => sum + current.cost, 0);
  }
}


