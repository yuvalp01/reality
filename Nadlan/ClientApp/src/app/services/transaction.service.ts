import {Injectable, Inject} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITransaction } from '../shared/models';

@Injectable()
export class TransactionService
{

    constructor (private httpClient:HttpClient, @Inject('BASE_URL') private baseUrl:String)
    {       
    }

    getTransactions() : Observable<ITransaction[]>
    {
      return this.httpClient.get<ITransaction[]>(this.baseUrl + 'api/transactions');
    }

  getTransactionsByAccount(apartmentId:number, accountId:number): Observable<ITransaction[]> {
    return this.httpClient.get<ITransaction[]>(`${this.baseUrl}api/transactions/${apartmentId}/${accountId}`);
  }

    addTransaction(transaction:ITransaction) : Observable<ITransaction>
    {
      const options  = {headers: new HttpHeaders({'Content-Type':'application/json'})};
      return this.httpClient.post<ITransaction>(this.baseUrl + 'api/transactions',transaction, options);
      // .pipe(catchError(this.had))
      }
}
