import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IIncomeReport, IPurchaseReport, ISummaryReport, IApartment, IInvestorReportOverview, IEndOfTheYearCalc, ISoFarReport } from '../models';


@Injectable()
export class ReportService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
  }
  getSummaryReport(apartmentId: number, year: number): Observable<ISummaryReport> {
    let url = this.baseUrl + `api/reports/GetSummaryReport/${apartmentId}/${year}`;
    return this.httpClient.get<ISummaryReport>(url);
  }

  GetSoFarReport(apartmentId: number, year: number): Observable<ISoFarReport> {
    let url = this.baseUrl + `api/reports/GetSoFarReport/${apartmentId}/${year}`;
    return this.httpClient.get<ISoFarReport>(url);
  }


  getPurchaseReport(apartmentId: number): Observable<IPurchaseReport> {
    let url = this.baseUrl + `api/reports/GetPurchaseReport/${apartmentId}`;
    return this.httpClient.get<IPurchaseReport>(url);
  }

  getIncomeReport(apartmentId: number, year: number): Observable<IIncomeReport> {

    let url = this.baseUrl + `api/reports/GetIncomeReport/${apartmentId}/${year}`;
    return this.httpClient.get<IIncomeReport>(url);
  }


  getApartmentInfo(apartmentId: number): Observable<IApartment> {

    let url = this.baseUrl + `api/reports/GetApartmentInfo/${apartmentId}`;
    return this.httpClient.get<IApartment>(url);
  }
}

