import {MatButtonModule} from '@angular/material/button';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatIconModule, MatNativeDateModule, MatFormFieldModule, MatInputModule, MatSelectModule} from '@angular/material'
import {MatDatepickerModule} from '@angular/material/datepicker';
import { NgModule } from '@angular/core';
import {TextFieldModule} from '@angular/cdk/text-field';
import {MatTableModule} from '@angular/material/table';
import {MatDialogModule} from '@angular/material/dialog';

@NgModule({
  imports: [
     MatButtonModule,
     MatCheckboxModule, 
     MatIconModule,
     MatDatepickerModule,
     MatNativeDateModule,
     MatFormFieldModule, 
     MatInputModule ,
     TextFieldModule,
     MatSelectModule,
     MatTableModule,
     MatDialogModule

    ],
  exports: [
     MatButtonModule,
     MatCheckboxModule,
     MatIconModule,
     MatDatepickerModule, 
     MatNativeDateModule,
     MatFormFieldModule,
     MatInputModule,
     TextFieldModule,
     MatSelectModule,
     MatTableModule,
     MatDialogModule
      ],
})
export class MyOwnCustomMaterialModule { }