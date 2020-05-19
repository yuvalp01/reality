import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITransaction } from '../models';

@Injectable()
export class TransactionService {


  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: String) {
  }

  getTransactions(): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(this.baseUrl + 'api/transactions');

    //let options = new HttpHeaders().set('Authorization', 'Bearer ' + this.securityService.securityObject.bearerToken);
    //return this.httpClient.get<ITransaction[]>(this.baseUrl + 'api/transactions', { headers: options });
  }

  getTransactionById(transactionId: number): Observable<ITransaction> {
    return this.httpClient.get<ITransaction>(`${this.baseUrl}api/transactions/${transactionId}`);
  }


  getTransactionsByAccount(apartmentId: number, accountId: number, isPurchaseCost: boolean, year: number = 0): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/${apartmentId}/${accountId}/${isPurchaseCost}/${year}`);
  }

  addTransaction(transaction: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<ITransaction>(this.baseUrl + 'api/transactions', transaction, options);
    // .pipe(catchError(this.had))
  }

  updateTransaction(transaction: ITransaction): any {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/${transaction.id}`, transaction, options);
  }

  confirmTransaction(transactionId: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/confirm`, transactionId, options);
  }

  deteleTransaction(transactionId: any): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.delete<ITransaction>(this.baseUrl + `api/transactions/${transactionId}`, options);
  }

}
