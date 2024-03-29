import { Injectable, Inject, DebugElement } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ITransaction } from '../models';
import { Observable } from 'rxjs';
import { strictEqual } from 'assert';
import { debounce } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
    this.controller = "api/expenses";
  }

  controller: string;



  getExpenses(monthsBack: number): Observable<ITransaction[]> {
    let url = `${this.baseUrl + this.controller}/list/${monthsBack}`;
    return this.httpClient.get<ITransaction[]>(url);
  }


  getExpense(transactionId: number): Observable<ITransaction> {
    let url = `${this.baseUrl + this.controller}/${transactionId}`
    return this.httpClient.get<ITransaction>(url);
    //return this.httpClient.get<ITransaction>(`${this.baseUrl}api/transactions/GetExpenses/${transactionId}`);
  }

  addExpense(transaction: ITransaction): Observable<ITransaction> {
    //(Stalla)
    transaction.createdBy = 2;
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    let url = `${this.baseUrl + this.controller}`
    //return this.httpClient.post<ITransaction>(`${this.baseUrl}api/transactions/PostExpenses`, transaction, options);
    return this.httpClient.post<ITransaction>(url, transaction, options);
  }

  receiveCash(transaction: ITransaction): Observable<ITransaction> {
    //(Stalla)
    transaction.createdBy = 2;
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    let url = `${this.baseUrl + this.controller}/ReceiveCash`
    return this.httpClient.post<ITransaction>(url, transaction, options);
  }

  updateExpense(transaction: ITransaction): Observable<ITransaction> {
    //Do not assign here createdBy. We want to preserve the original createdBy. 
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    let url = `${this.baseUrl + this.controller}`
    return this.httpClient.put<ITransaction>(url, transaction, options);
    //return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/PutExpenses`, transaction, options);
  }

  deleteExpense(transactionId: number): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    let url = `${this.baseUrl + this.controller}/${transactionId}`;
    return this.httpClient.delete<ITransaction>(url, options);
    //return this.httpClient.delete<ITransaction>(`${this.baseUrl}api/transactions/${transactionId}`, options);
  }

  getExpensesBalance(): Observable<number> {

    let url = `${this.baseUrl + this.controller}/GetExpensesBalance/`;
    return this.httpClient.get<number>(url);
  }

}
