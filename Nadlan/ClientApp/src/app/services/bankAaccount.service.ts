import { Injectable, Inject } from '@angular/core';
import { IAccount, IBankAccount } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class BankAccountService {
    accounts: IAccount[];

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    getBankAccounts(): Observable<IBankAccount[]> {
        return this.httpClient.get<IBankAccount[]>(this.baseUrl + 'api/bankAccounts');
    }

}
