import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITransaction } from '../models';

@Injectable()
export class TransactionService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getTransactions(): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(this.baseUrl + 'api/transactions');
  }

  getTransactionsByAccount(apartmentId: number, accountId: number, isPurchaseCost: boolean, year: number = 0): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/${apartmentId}/${accountId}/${isPurchaseCost}/${year}`);
  }

  //getExpenses(): Observable<ITransaction[]> {
  //  return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/GetExpenses`);
  //}


  //getExpense(transactionId: number): Observable<ITransaction> {
  //  return this.httpClient.get<ITransaction>(`${this.baseUrl}api/transactions/GetExpenses/${transactionId}`);
  //}

  addTransaction(transaction: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<ITransaction>(this.baseUrl + 'api/transactions', transaction, options);
    // .pipe(catchError(this.had))
  }



  //addExpense(transaction: ITransaction): Observable<ITransaction> {
  //  const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  //  return this.httpClient.post<ITransaction>(`${this.baseUrl}api/transactions/PostExpenses`, transaction, options);
  //}

  //updateExpense(transaction: ITransaction): Observable<ITransaction> {
  //  const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  //  return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/PutExpenses`, transaction, options);
  //}

  //deleteExpense(transactionId: number): Observable<{}> {
  //  const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  //  return this.httpClient.delete<ITransaction>(`${this.baseUrl}api/transactions/${transactionId}`, options);
  //}

  //confirmExpense(transactionId: ITransaction): Observable<ITransaction> {
  //  const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  //  return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/confirm`, transactionId, options);
  //}
}
