import { Injectable, Inject} from '@angular/core';
import { IAccount } from '../shared/models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class AccountService
{
    accounts: IAccount[];

    constructor (private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl:string)
    {
    } 

    getAccounts(): Observable<IAccount[]>
    {
       return this.httpClient.get<IAccount[]>(this.baseUrl + 'api/accounts');
    }
    addAccount(newAccount:IAccount) : Observable<IAccount>
    {
        let options = {headers: new HttpHeaders({'Content-Type':'application/json'})};
        return this.httpClient.post<IAccount>(this.baseUrl+ 'api/accounts',newAccount,options)
    }
}
