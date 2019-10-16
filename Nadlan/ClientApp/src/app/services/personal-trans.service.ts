import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPersonalTransaction } from '../models';

@Injectable()
export class PessonalTransactionService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getPesonalTrans(): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(this.baseUrl + 'api/personalTransactions');
  }

  getPesonalTransByStakeholder(stakeholderId: number): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(`${this.baseUrl}api/personalTransactions/${stakeholderId}`);
  }

  addPersonalTrans(transaction: IPersonalTransaction): Observable<IPersonalTransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<IPersonalTransaction>(this.baseUrl + 'api/personalTransactions', transaction, options);
  }

}
