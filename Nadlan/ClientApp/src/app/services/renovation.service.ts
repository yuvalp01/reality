import { Injectable, Inject } from '@angular/core';
import { IRenovationLine, IRenovationProject, IRenovationPayment, IRenovationProduct } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class RenovationService {


  options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  controller = 'api/renovation';

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  getRenovationProjects(): Observable<IRenovationProject[]> {
    return this.httpClient.get<IRenovationProject[]>(`${this.baseUrl}/api/renovation/projects`);
  }
  getRenovationProject(projectId): Observable<IRenovationProject> {
    return this.httpClient.get<IRenovationProject>(`${this.baseUrl}/api/renovation/project/${projectId}`);
  }
  getRenovationLines(projectId): Observable<IRenovationLine[]> {
    return this.httpClient.get<IRenovationLine[]>(`${this.baseUrl}/api/renovation/lines/${projectId}`);
  }

  getRenovationPayments(projectId): Observable<IRenovationPayment[]> {
    return this.httpClient.get<IRenovationPayment[]>(`${this.baseUrl}/api/renovation/payments/${projectId}`);
  }

  getRenovationPaymentById(paymentId): Observable<IRenovationPayment> {
    return this.httpClient.get<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment/${paymentId}`);
  }


  updateRenovationPayment(payment: IRenovationPayment): Observable<IRenovationPayment> {
    // const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment`, payment, this.options);
  }

  makePayment(payment: IRenovationPayment): Observable<number> {
    // const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<number>(`${this.baseUrl}/api/renovation/makePayment`, payment, this.options);
  }

  cancelPayment(paymentId: number): Observable<number> {
    // const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<number>(`${this.baseUrl}/api/renovation/cancelPayment`, paymentId, this.options);
  }

  confirmPayment(paymentId: number): Observable<IRenovationPayment> {
    // const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.httpClient.put<IRenovationPayment>(`${this.baseUrl}/api/renovation/confirmPayment`, paymentId, this.options);
  }

  updateLine(line: IRenovationLine): Observable<IRenovationLine> {
    return this.httpClient.put<IRenovationLine>(`${this.baseUrl}/api/renovation/line`, line, this.options);
  }
  addRenovationPayment(payment: IRenovationPayment): Observable<IRenovationPayment> {
    return this.httpClient.post<IRenovationPayment>(`${this.baseUrl}/api/renovation/payment`, payment, this.options);
  }
  deletePayment(paymentId: number): Observable<number> {
    return this.httpClient.put<number>(`${this.baseUrl}/api/renovation/deletePayment`, paymentId, this.options);
  }


  deleteLine(lineId: number): Observable<number> {
    return this.httpClient.delete<number>(`${this.baseUrl}/${this.controller}/lines/${lineId}`, this.options);

  }

  updateProject(project: IRenovationProject) {
    return this.httpClient.put<IRenovationProject>(`${this.baseUrl}/api/renovation/project`, project, this.options);

  }



  addLine(line: IRenovationLine): Observable<IRenovationLine> {
    return this.httpClient.post<IRenovationLine>(`${this.baseUrl}/api/renovation/lines`, line, this.options);
  }




  getProducts(): Observable<IRenovationProduct[]> {
    return this.httpClient.get<IRenovationProduct[]>(`${this.baseUrl}/${this.controller}/products`)
  }

  getProductByType(itemType: string):Observable<IRenovationProduct[]> {
    return this.httpClient.get<IRenovationProduct[]>(`${this.baseUrl}/${this.controller}/products/byType/${itemType}`);
  }

  getProductById(id: number):Observable<IRenovationProduct> {
    return this.httpClient.get<IRenovationProduct>(`${this.baseUrl}/${this.controller}/products/${id}`);
  }




  deleteProduct(id: number):Observable<{}> {
    return this.httpClient.delete<IRenovationProduct>(`${this.baseUrl}/${this.controller}/products/${id}`, this.options);
  }

  addProduct(product: IRenovationProduct): Observable<IRenovationProduct> {
    return this.httpClient.post<IRenovationProduct>(`${this.baseUrl}/${this.controller}/products`,product, this.options);
  }
  updateProduct(product: IRenovationProduct): Observable<IRenovationProduct> {
    return this.httpClient.put<IRenovationProduct>(`${this.baseUrl}/${this.controller}/products`, product, this.options);
  }

}
