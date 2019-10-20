import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPersonalTransaction, IStakeholder, IInvestorReportOverview } from '../models';

@Injectable({
  providedIn: 'root'
})
export class PersonalTransService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getPesonalTrans(): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(this.baseUrl + 'api/personalTransactions');
  }
  getPesonalTransById(transId): Observable<IPersonalTransaction> {
    return this.httpClient.get<IPersonalTransaction>(`${this.baseUrl}api/personalTransactions/${transId}`);
  }
  getStakeholders(): Observable<IStakeholder[]> {
    return this.httpClient.get<IStakeholder[]>(this.baseUrl + 'api/personalTransactions/getStakeholders');
  }

  getPesonalTransByStakeholder(stakeholderId: number): Observable<IPersonalTransaction[]> {
    return this.httpClient.get<IPersonalTransaction[]>(`${this.baseUrl}api/personalTransactions/GetByStakeholderId/${stakeholderId}`);
  }

  addPersonalTrans(transaction: IPersonalTransaction): Observable<IPersonalTransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<IPersonalTransaction>(this.baseUrl + 'api/personalTransactions', transaction, options);
  }
  editPersonalTrans(transaction: IPersonalTransaction): Observable<IPersonalTransaction> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IPersonalTransaction>(this.baseUrl + 'api/personalTransactions', transaction, options);
  }
  detelePersonalTrans(transactionId: number): Observable<{}> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.delete<IPersonalTransaction>(this.baseUrl + `api/personalTransactions/${transactionId}`, options);
  }
  getPersonalBalance(stakeholderId): Observable<number> {

    let url = `${this.baseUrl}api/reports/GetPersonalBalance/${stakeholderId}`;
    return this.httpClient.get<number>(url);
  }
  getInvestorReport(investorAcountId: number): Observable<IInvestorReportOverview> {

    let url = this.baseUrl + `api/reports/GetInvestorReport/${investorAcountId}`;
    return this.httpClient.get<IInvestorReportOverview>(url);
  }
}
