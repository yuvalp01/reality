import { Component, OnInit, NgZone, Inject, ViewChild, EventEmitter, Output } from '@angular/core';
import { RenovationService } from 'src/app/services/renovation.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MAT_DIALOG_DATA } from '@angular/material';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IRenovationProduct } from 'src/app/models';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private _ngZone: NgZone,
    private snackBar: MatSnackBar,
    private renovationService: RenovationService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
  formTitle: string;
  productForm: FormGroup;
  product: IRenovationProduct;
  @Output() refreshEmitter = new EventEmitter();
  ngOnInit() {
    this.productForm = this.formBuilder.group({
      id:0,
      name: ['', Validators.required],
      description: [''],
      price: [0],
      store: [null],
      link: [null],
      serialNumber: [null],
      photoUrl: null,

    });
    //add new
    if (this.data==0) {
      this.formTitle = "Add new";
    }
    else {
      this.formTitle = "Edit Product";
      this.loadData();
    }

  }
  loadData() {
    this.renovationService.getProductById(this.data)
      .subscribe({
        next: result => {
          this.product = result;
          this.productForm.setValue(this.product);
        },
        error: err => console.error(err)
      });
  }
  save() {
    if (this.productForm.valid && this.productForm.dirty) {
      const t: IRenovationProduct = { ...this.product, ...this.productForm.value }
      //If new
      if (this.data) {
        {
          this.renovationService.updateProduct(t)
            .subscribe({
              next: result => this.onSaveComplete('Updated'),
              error: err => console.error(err)
            });

        }
      }
      else {
        this.renovationService.addProduct(t)
          .subscribe({
            next: result => this.onSaveComplete('Added'),
            error: err => console.error(err)
          });
      }
    }
    else {
      return;
    }
  }

  onSaveComplete(_action: string) {
    let snackBarRef = this.snackBar.open(`Product`, _action, { duration: 2000 });
    this.refreshEmitter.emit();
  }


  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
