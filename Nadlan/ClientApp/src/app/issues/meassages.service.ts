import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IIssue, IMessage } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  controller = 'api/messages';
  options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: String) { }

  getMessages(tableName:string,parentId:number): Observable<IMessage[]> {
    return this.httpClient.get<IMessage[]>(`${this.baseUrl}${this.controller}/byParent/${tableName}/${parentId}`);
  }

  addNewMessage(message: IMessage): Observable<IMessage> {
    return this.httpClient.post<IMessage>(`${this.baseUrl}${this.controller}`, message, this.options);
  }
  updateMessage(message: IMessage): Observable<{}> {
    return this.httpClient.put<IMessage>(`${this.baseUrl}${this.controller}`, message, this.options);
  }
  markAsRead(message: IMessage): Observable<{}> {
    return this.httpClient.put<IMessage>(`${this.baseUrl}${this.controller}/markAsRead`, message, this.options);
  }
}
