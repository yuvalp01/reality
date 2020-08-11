import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApartmentReportsComponent } from '../reports/apartment-reports.component';
import { MyOwnCustomMaterialModule } from './cusotom-material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MinusSignToParens } from './minusSignToParens';
import { HasClaimDirective } from '../security/has-claim.directive';



@NgModule({
  declarations: [
    MinusSignToParens,
    ApartmentReportsComponent, 
    HasClaimDirective
  ],
  imports: [
    CommonModule,
    MyOwnCustomMaterialModule,
    ReactiveFormsModule,
    FormsModule,
    
  ],
  exports: [
    CommonModule,
    MyOwnCustomMaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MinusSignToParens,
    ApartmentReportsComponent,
    HasClaimDirective
    ],
})
export class SharedModule { }
