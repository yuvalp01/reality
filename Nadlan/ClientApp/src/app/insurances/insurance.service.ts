import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IInsurance } from '../models';

@Injectable({
  providedIn: 'root'
})
export class InsuranceService {

  controller = 'api/insurances';
  options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  getAll(): Observable<IInsurance[]> {
    return this.httpClient.get<IInsurance[]>(`${this.baseUrl}${this.controller}`);
  }
  getById(id: number): Observable<IInsurance> {
    return this.httpClient.get<IInsurance>(`${this.baseUrl}${this.controller}/${id}`);
  }
  add(insurance: IInsurance): Observable<{}> {
    return this.httpClient.post<IInsurance>(`${this.baseUrl}${this.controller}`, insurance, this.options);
  }
  update(insurance: IInsurance, id: number): Observable<{}> {
    return this.httpClient.put<IInsurance>(`${this.baseUrl}${this.controller}/${id}`, insurance, this.options);
  }
  delete(id: number): Observable<{}> {
    return this.httpClient.delete<IInsurance>(`${this.baseUrl}${this.controller}/${id}`, this.options);
  }
}
