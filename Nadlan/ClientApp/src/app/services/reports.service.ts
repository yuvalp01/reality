import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IIncomeReport, IPurchaseReport, ISummaryReport } from '../shared/models';
import { strictEqual } from 'assert';

@Injectable()
export class ReportService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getSummaryReport(apartmentId: number): Observable<ISummaryReport> {
    let url = this.baseUrl + 'api/reports/GetSummaryReport/' + apartmentId;
    return this.httpClient.get<ISummaryReport>(url);
  }

  getPurchaseReport(apartmentId: number): Observable<IPurchaseReport> {
    let url = this.baseUrl + 'api/reports/GetPurchaseReport/' + apartmentId;
    return this.httpClient.get<IPurchaseReport>(url);
  }

  getIncomeReport(apartmentId: number, year: number): Observable<IIncomeReport> {

    //let url = this.baseUrl + 'api/GetIncomeReports/' + apartmentId + '/' + year;
    let url = this.baseUrl + 'api/reports/GetIncomeReport/' + apartmentId + '/' + year;
    return this.httpClient.get<IIncomeReport>(url);
  }

}
