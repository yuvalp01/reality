import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SecurityService } from './security.service';
import { LayoutModule } from '@angular/cdk/layout';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private securityService: SecurityService,
    private router: Router) {

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    let claimType: string = route.data["claimType"];
    if (this.securityService.securityObject.isAuthenticated
      && this.securityService.hasClaim(claimType)) {
      //let stakeholderId = route.paramMap.get('stakeholderId');
      //if (stakeholderId) {
      //  let isTrue = false;
      //  switch (stakeholderId) {
      //    case '101':
      //      isTrue = claimType.includes('ohad');
      //      return isTrue;
      //    default:
      //  }
      //}
      //else {
        return true;
      //}
    }
    else {
      this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    }
  }

}
