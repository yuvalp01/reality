import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IIssue, IMessage } from '../models';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class IssuesService {

  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: String) { }


  getOpenIssues(): Observable<IIssue[]> {    
    return this.httpClient.get<IIssue[]>(this.baseUrl + 'api/issues/true');
  }
  getOpenIssuesWithMessages(): Observable<IMessage[]> {    
    return this.httpClient.get<IMessage[]>(this.baseUrl + 'api/issues/getMessages/true');
  }
}
