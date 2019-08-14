import {Component, OnInit} from '@angular/core';
import { IApartment } from '../shared/models';
import { ApartmentService } from '../services/apartment.service';


@Component({
    templateUrl:'./fetch-apart.component.html',
})
export class ApartmentListComponent implements OnInit
{
    constructor (private apartmentService:ApartmentService)
    {
    }
    apartments:IApartment[] ;
    ngOnInit(): void {
        this.apartmentService.getApartments().subscribe(result=>{this.apartments=result}, error=>console.error(error));
    }

}
