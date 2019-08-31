import { Injectable, Inject} from '@angular/core';
import { ILine } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class RenovationService
{
  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl:string)  {

  }

  renovationLines: ILine[];

  getRenovationLines(apartmentId):Observable<ILine[]> {
    return this.httpClient.get<ILine[]>(`${this.baseUrl}/api/renovation/renovationLines/${apartmentId}`);
  }

}
