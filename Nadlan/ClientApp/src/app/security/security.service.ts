import { Injectable, Inject } from '@angular/core';
import { AppUserAuth } from './app.user.auth';
import { AppUser } from './app.user';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
//import { LOGIN_MOCKS } from './login.mocks.';
import { tap } from 'rxjs/operators';


const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  securityObject: AppUserAuth = new AppUserAuth();
  controller: string;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: String) {
    this.controller = "api/security/login";
  }

  login(entity: AppUser): Observable<AppUserAuth> {
    this.resetSecurityObject();
    let url = `${this.baseUrl + this.controller}`

    return this.httpClient.post<AppUserAuth>(url, entity, options).pipe(
      tap(resp => {
        Object.assign(this.securityObject, resp);
        localStorage.setItem('bearerToken', resp.bearerToken);
      }));

    //Object.assign(this.securityObject, LOGIN_MOCKS.find(user => user.userName.toLowerCase() ===
    //  entity.userName.toLowerCase()));
    //if (this.securityObject.userName != "") {
    //  localStorage.setItem("bearerToken", this.securityObject.bearerToken);
    //}
    //return of<AppUserAuth>(this.securityObject);
  }
  logout(): void {
    this.resetSecurityObject();
  }
  resetSecurityObject(): void {
    this.securityObject.userName = "";
    this.securityObject.bearerToken = "";
    this.securityObject.isAuthenticated = false;

    this.securityObject.claims = [];
    //this.securityObject.canViewReports = false;
    //this.securityObject.canConfirmExpenses = false;
    //this.securityObject.canAccessRest = false;
    //this.securityObject.canViewExpenses = false;
    //this.securityObject.canViewTransactions = false;

    localStorage.removeItem("bearerToken")

  }

  hasClaim(claimType: string, claimValue?: string): any {
    let isValid: boolean = false;
    if (typeof claimType==='string') {
      isValid = this.isClaimValid(claimType, claimValue);
    }
    else {
      let claims: string[] = claimType;
      if (claims) {
        for (var i = 0; i < claims.length; i++) {
          isValid = this.isClaimValid(claims[i]);
          if (isValid) {
            break;
          }
        }
      }
    }
    return isValid;
    //return this.isClaimValid(claimType, claimValue);
  }

  private isClaimValid(claimType: string, claimValue?: string): boolean {
    let isValid: boolean = false;
    let auth: AppUserAuth = this.securityObject;
    if (auth) {
      if (claimType.indexOf(':') >= 0) {
        let words: string[] = claimType.split(':');
        claimType = words[0].toLowerCase();
        claimValue = words[1];
      }
      else {
        claimType = claimType.toLowerCase();
        claimValue = claimValue ? claimValue : 'true';
      }
      let claimFound = auth.claims.find(a =>
        a.claimType.toLowerCase() == claimType
        && a.claimValue.toLowerCase() == claimValue);



      isValid = claimFound != null;
    }
    return isValid;
  }
}
