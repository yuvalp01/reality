import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IContract } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ContractService {

  controller = 'api/contracts';
  options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  getAll(): Observable<IContract[]> {
    return this.httpClient.get<IContract[]>(`${this.baseUrl}${this.controller}`);
  }
  getById(id:number):Observable<IContract>{
    return this.httpClient.get<IContract>(`${this.baseUrl}${this.controller}/${id}`);
  }
  add(contract:IContract):Observable<{}>{
    return this.httpClient.post<IContract>(`${this.baseUrl}${this.controller}`,contract,this.options);
  }
  update(contract:IContract):Observable<{}>{
    return this.httpClient.put<IContract>(`${this.baseUrl}${this.controller}`,contract,this.options);
  }
  delete(id:number):Observable<{}>{
    return this.httpClient.delete<IContract>(`${this.baseUrl}${this.controller}/${id}`,this.options);
  }
}
