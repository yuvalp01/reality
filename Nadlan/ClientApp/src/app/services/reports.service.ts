import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IIncomeReport, IPurchaseReport, ISummaryReport, IApartment, IInvestorReportOverview } from '../models';
import { strictEqual } from 'assert';

@Injectable()
export class ReportService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }

  getSummaryReport(apartmentId: number): Observable<ISummaryReport> {
    let url = this.baseUrl + `api/reports/GetSummaryReport/${apartmentId}`;
    return this.httpClient.get<ISummaryReport>(url);
  }

  getPurchaseReport(apartmentId: number): Observable<IPurchaseReport> {
    let url = this.baseUrl + `api/reports/GetPurchaseReport/${apartmentId}`;
    return this.httpClient.get<IPurchaseReport>(url);
  }

  getIncomeReport(apartmentId: number, year: number): Observable<IIncomeReport> {

    let url = this.baseUrl + `api/reports/GetIncomeReport/${apartmentId}/${year}`;
    return this.httpClient.get<IIncomeReport>(url);
  }
  getAccountBalance(accountId: number): Observable<number> {

    //let url = this.baseUrl + 'api/GetIncomeReports/' + apartmentId + '/' + year;
    let url = this.baseUrl + `api/reports/GetBalance/${accountId}`;
    return this.httpClient.get<number>(url);
  }
  getExpensesBalance(): Observable<number> {

    let url = `${this.baseUrl}api/reports/GetExpensesBalance/`;
    return this.httpClient.get<number>(url);
  }
  getApartmentInfo(apartmentId: number): Observable<IApartment> {

    let url = this.baseUrl + `api/reports/GetApartmentInfo/${apartmentId}`;
    return this.httpClient.get<IApartment>(url);
  }
  getInvestorReport(investorAcountId: number): Observable<IInvestorReportOverview> {

    let url = this.baseUrl + `api/reports/GetInvestorReport/${investorAcountId}`;
    return this.httpClient.get<IInvestorReportOverview>(url);
  }
}
