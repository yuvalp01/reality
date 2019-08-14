import { Injectable, Inject } from "@angular/core";
import { IApartment } from "../shared/models";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";


@Injectable()
export class ApartmentService
{

    constructor (private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string)
    {

    }

    apartments:IApartment[] ;


    getApartments(): Observable<IApartment[]>
    {
       return this.httpClient.get<IApartment[]>(this.baseUrl + 'api/apartments');
    }

    addApartment(apartment):Observable<IApartment>
    {
        console.log(apartment);  
        let options = {headers: new HttpHeaders({'Content-Type':'application/json'})};
        return  this.httpClient.post<IApartment>(this.baseUrl + 'api/apartments',apartment, options);
    }

}
