import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IIssue, IMessage } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class IssuesService {

  controller = 'api/issues';
  options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: String) { }


  getOpenIssues(stakeholderId: number): Observable<IIssue[]> {
    return this.httpClient.get<IIssue[]>(`${this.baseUrl}${this.controller}/false/${stakeholderId}`);

  }
  addNewIssue(issue: IIssue): Observable<{}> {
    return this.httpClient.post<IIssue>(`${this.baseUrl}${this.controller}`, issue, this.options);
  }
  updateIssue(issue: IIssue): Observable<{}> {
    return this.httpClient.put<IIssue>(`${this.baseUrl}${this.controller}`, issue, this.options);
  }
  delete(id: number) {
    return this.httpClient.delete<IIssue>(`${this.baseUrl}${this.controller}/${id}`, this.options);
  }
}
