import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IIncomeReport, IPurchaseReport } from '../shared/models';
import { strictEqual } from 'assert';

@Injectable()
export class ReportService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getPurchaseReport(apartmentId: number): Observable<IPurchaseReport> {
    return this.httpClient.get<IPurchaseReport>(this.baseUrl + 'api/reports/GetPurchaseReport/' + apartmentId);
  }

  getIncomeReports(apartmentId: number, year: number): Observable<IIncomeReport> {

    let url = this.baseUrl + 'api/GetIncomeReports/' + apartmentId + '/' + year;
    console.log(url);
    return this.httpClient.get<IIncomeReport>(this.baseUrl + 'api/reports/GetIncomeReports/'+apartmentId+'/'+year);
  }

}
