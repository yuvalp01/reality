import { Component, OnInit, NgZone, Inject, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { RenovationService } from 'src/app/services/renovation.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef, _countGroupLabelsBeforeOption } from '@angular/material';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { IRenovationProduct } from 'src/app/models';
import { staticViewQueryIds } from '@angular/compiler';
// import { DriveLinkGenComponent } from 'src/app/shared/drive-link-gen/drive-link-gen.component';

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
    private dialogRef:MatDialogRef<ProductFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
  formTitle: string;
  productForm: FormGroup;
  // product: IRenovationProduct;
  @Output() refreshEmitter = new EventEmitter();

  // @ViewChild('driveLinkGen',{static:false}) driveLinkGen: DriveLinkGenComponent;

  @ViewChild('firstField',{static:false}) firstField: ElementRef;


  ngOnInit() {
    this.productForm = this.formBuilder.group({
      id: [0],
      name: ['', Validators.required],
      description: [''],
      itemType:['',Validators.required],
      price: [0, Validators.required],
      store: [null],
      link: [null],
      serialNumber: [null],
      photoUrl: null,
      isDeleted:false,
    });
    //default: add new
    this.formTitle = "Add New Product";
    if (this.data) {
      this.formTitle = "Edit Product";
      this.loadData(this.data as IRenovationProduct);
    }

  }
  loadData(item:IRenovationProduct) {
    this.productForm.patchValue(item);
  }
  save() {
    if (this.productForm.valid && this.productForm.dirty) {
     // const t: IRenovationProduct = { ...this.product, ...this.productForm.value }
      let product: IRenovationProduct = Object.assign({}, this.productForm.value);
      //If new
      if (this.data) {
        {
          this.renovationService.updateProduct(product)
            .subscribe({
              next: result => this.onSaveComplete('Updated'),
              error: err => console.error(err)
            });

        }
      }
      else {
        this.renovationService.addProduct(product)
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
    // if(_action=="Updated")
    // {
      this.dialogRef.close();
    // }
    // else{
    //   this.productForm.reset();
    //   this.firstField.nativeElement.focus();
    // }
   
  }


  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
