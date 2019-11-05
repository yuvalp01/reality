import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ITransaction } from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getExpenses(): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/GetExpenses`);
  }


  getExpense(transactionId: number): Observable<ITransaction> {
    return this.httpClient.get<ITransaction>(`${this.baseUrl}api/transactions/GetExpenses/${transactionId}`);
  }

  addExpense(transaction: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<ITransaction>(`${this.baseUrl}api/transactions/PostExpenses`, transaction, options);
  }

  updateExpense(transaction: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/PutExpenses`, transaction, options);
  }

  deleteExpense(transactionId: number): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.delete<ITransaction>(`${this.baseUrl}api/transactions/${transactionId}`, options);
  }

  //confirmExpense(transactionId: ITransaction): Observable<ITransaction> {
  //  const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  //  return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/confirm`, transactionId, options);
  //}
}
