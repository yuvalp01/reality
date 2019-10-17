import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPersonalTransaction, IStakeholder } from '../models';

@Injectable({
  providedIn: 'root'
})
export class PersonalTransService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getPesonalTrans(): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(this.baseUrl + 'api/personalTransactions');
  }

  getStakeholders(): Observable<IStakeholder[]> {
    return this.httpClient.get<IStakeholder[]>(this.baseUrl + 'api/personalTransactions/getStakeholders');
  }

  getPesonalTransByStakeholder(stakeholderId: number): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(`${this.baseUrl}api/personalTransactions/GetPersonalTransactionByStakeholderId/${stakeholderId}`);
  }

  addPersonalTrans(transaction: IPersonalTransaction): Observable<IPersonalTransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<IPersonalTransaction>(this.baseUrl + 'api/personalTransactions', transaction, options);
  }
  editPersonalTrans(transaction: IPersonalTransaction): Observable<IPersonalTransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<IPersonalTransaction>(this.baseUrl + 'api/personalTransactions', transaction, options);
  }
  detelePersonalTrans(transactionId: number): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.delete<IPersonalTransaction>(this.baseUrl + `api/personalTransactions/${transactionId}`, options);
  }
}
