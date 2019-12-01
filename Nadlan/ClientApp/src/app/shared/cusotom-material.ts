import {MatButtonModule} from '@angular/material/button';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatIconModule, MatNativeDateModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCardModule, MatGridListModule, MatSidenavModule, MatListModule, MatSortModule, MatDividerModule, MatTabsModule, MatTooltipModule, MatExpansionModule, MatTreeModule, MatBadgeModule, MatSnackBarModule, MatMenuModule} from '@angular/material'
import {MatDatepickerModule} from '@angular/material/datepicker';
import { NgModule } from '@angular/core';
import {TextFieldModule} from '@angular/cdk/text-field';
import {MatTableModule} from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

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
     MatDialogModule,
    MatCardModule,
    MatGridListModule,
    MatSidenavModule,
    MatListModule,
    MatSortModule,
    MatDividerModule,
    MatTabsModule,
    MatTooltipModule,
    MatExpansionModule,
    MatTreeModule,
    MatBadgeModule,
    MatSnackBarModule,
    MatMenuModule,
    MatSlideToggleModule

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
     MatDialogModule,
    MatCardModule,
    MatGridListModule,
    MatSidenavModule,
    MatListModule,
    MatSortModule,
    MatDividerModule,
    MatTabsModule,
    MatTooltipModule,
    MatExpansionModule,
    MatTreeModule,
    MatBadgeModule,
    MatSnackBarModule,
    MatMenuModule,
    MatSlideToggleModule
      ],
})
export class MyOwnCustomMaterialModule { }
