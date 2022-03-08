import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITransaction, IFilter } from '../models';

@Injectable()
export class TransactionService {


  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: String) {
  }


  getTransactions_(filter: IFilter): Observable<ITransaction[]> {
    let params = new URLSearchParams();
    for (let key in filter) {
      params.set(key, filter[key])
    }
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions?${params}`);
  }

  getFilteredTransactions(filter: IFilter): Observable<ITransaction[]> {
    let params = new URLSearchParams();
    for (let key in filter) {
      params.set(key, filter[key])
    }
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/GetFiltered?${params}`);
  }

  getTransactions(monthsBack: number): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/list/${monthsBack}`);

    //let options = new HttpHeaders().set('Authorization', 'Bearer ' + this.securityService.securityObject.bearerToken);
    //return this.httpClient.get<ITransaction[]>(this.baseUrl + 'api/transactions', { headers: options });
  }

  getTransactionById(transactionId: number): Observable<ITransaction> {
    return this.httpClient.get<ITransaction>(`${this.baseUrl}api/transactions/${transactionId}`);
  }


  getTransactionsByAccount(apartmentId: number, accountId: number, isPurchaseCost: boolean, year: number = 0): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/${apartmentId}/${accountId}/${isPurchaseCost}/${year}`);
  }

  getByPersonalTransactionId(personalTransactionId: number): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/getByPersonalTransactionId/${personalTransactionId}`);
  }

  getPendingExpensesForInvestor(stakeholderId: number): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/getPendingExpensesForInvestor/${stakeholderId}`);
  }
  getPendingExpensesForApartment(apartmentId: number, year: number): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/getPendingExpensesForApartment/${apartmentId}/${year}`);
  }




  addTransaction(transaction: ITransaction): Observable<ITransaction> {
    //(Yuval)
    transaction.createdBy = 1;
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<ITransaction>(this.baseUrl + 'api/transactions', transaction, options);
    // .pipe(catchError(this.had))
  }

  updateTransaction(transaction: ITransaction): any {
    //Do not assign here createdBy. We want to preserve the original createdBy. 
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/${transaction.id}`, transaction, options);
  }

  confirmTransaction(transactionId: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/confirm`, transactionId, options);
  }

  payUnpayTransaction(transactionId: ITransaction): Observable<ITransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<ITransaction>(`${this.baseUrl}api/transactions/payUnpay`, transactionId, options);
  }


  deteleTransaction(transactionId: any): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.delete<ITransaction>(this.baseUrl + `api/transactions/${transactionId}`, options);
  }

}
