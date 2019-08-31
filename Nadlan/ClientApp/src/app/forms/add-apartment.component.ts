import {  Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { ApartmentService } from "../services/apartment.service";
import { IApartment } from "../models";
import { Router } from "@angular/router";
import { KeyValue } from "@angular/common";
import { resource } from "selenium-webdriver/http";


@Component({
    templateUrl:'./add-apartment.component.html'
})
export class  AddApartmentForm implements OnInit    
{

    apartmentForm:FormGroup;
    statuses:KeyValue<number,string>[];

    constructor(private apartmentService:ApartmentService, private router:Router)
    {

    }

    ngOnInit(): void {

        let address = new FormControl();
        let status = new FormControl(0);
        this.apartmentForm = new FormGroup(
            {
                address: address,
                status: status
            });
  
            this.statuses = [{key:1,value:'status1'},{key:2,value:'status2'}];
            console.log(this.statuses);
    }



    saveApartment(formValues)
    {
        const newApartment:IApartment = Object.assign({},formValues);
        console.log(newApartment.status);
        this.apartmentService.addApartment(newApartment).subscribe(result=> {
            console.log(result.id);
        } );
    }

}
