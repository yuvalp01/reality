import { Injectable, Inject} from '@angular/core';
import { ILine, IItemDto, IRenovationLine, IRenovationProject, IRenovationPayment } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class RenovationService
{
  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl:string)  {

  }

  getRenovationProjects(): Observable<IRenovationProject[]> {
    return this.httpClient.get<IRenovationProject[]>(`${this.baseUrl}/api/renovation/projects`);
  }
  //renovationLines: ILine[];
  getRenovationLinesNew(projectId): Observable<IRenovationLine[]> {
    return this.httpClient.get<IRenovationLine[]>(`${this.baseUrl}/api/renovation/lines/${projectId}`);
  }

  getRenovationPayments(projectId): Observable<IRenovationPayment[]> {
    return this.httpClient.get<IRenovationPayment[]>(`${this.baseUrl}/api/renovation/payments/${projectId}`);
  }

  getRenovationPaymentById(paymentId): Observable<IRenovationPayment> {
    return this.httpClient.get<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment/${paymentId}`);
  }


  updateRenovationPayment(payment: IRenovationPayment): Observable<IRenovationPayment> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment`,payment,options);
  }

  makePayment(payment: IRenovationPayment): Observable<number> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<number>(`${this.baseUrl}/api/renovation/makePayment`,payment,options);
  }


  confirmPayment(paymentId: number): Observable<IRenovationPayment> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IRenovationPayment>(`${this.baseUrl}/api/renovation/confirmPayment`,paymentId,options);
  }
  deletePayment(paymentId: number): Observable<IRenovationPayment> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IRenovationPayment>(`${this.baseUrl}/api/renovation/deletePayment`,paymentId,options);
  }

  addRenovationPayment(payment: IRenovationPayment): Observable<IRenovationPayment> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.post<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment`,payment, options);
  }




  getRenovationLines(apartmentId):Observable<ILine[]> {
    return this.httpClient.get<ILine[]>(`${this.baseUrl}/api/renovation/renovationLines/${apartmentId}`);
  }

  getRenovationItems(apartmentId): Observable<IItemDto[]> {
    return this.httpClient.get<IItemDto[]>(`${this.baseUrl}/api/renovation/renovationItems/${apartmentId}`);
  }

}
