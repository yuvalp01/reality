import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SecurityService } from '../security/security.service';
import { AppUserAuth } from '../security/app.user.auth';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  role: number = 0;
  userId: number = 0;
  securityObject: AppUserAuth = null;
    constructor(private securityService: SecurityService, private router: Router) {
      this.securityObject = this.securityService.securityObject;
  }

  logout(): void {
    this.securityService.logout();
  }

  ngOnInit() {
    //if (this.securityObject.isAuthenticated) {
    //  //this.router.navigateByUrl(`/expenses`);
    //  var xx = this.securityObject;
    //}


    //let xxx = this.route.snapshot.queryParams['role'];
    //const queryParams = this.route.snapshot.queryParams;
    //let yyy = queryParams.role;
    ////this.route.queryParamMap.subscribe(params => {
    ////  this.role =  +params.get('role');
    ////})
    // this.role = 1;
    //this.userId = 107;
    //this.userId = 199;
    //this.configurePage(this.userId);
    //window.sessionStorage.setItem("role", this.role.toString());

  }

  //configurePage(userId:number) {
  //  switch (userId) {
  //    case 199:
  //      this.role = 1;
  //      this.router.navigateByUrl(`/fetch-transactions`);
  //      //this.router.navigateByUrl(`/reports/1`);
  //      //this.router.navigateByUrl(`/personal-admin/101`);
        
  //      break;
  //    case 107:
  //      this.role = 2;
  //      this.router.navigateByUrl(`/expenses`);
  //      break;
  //    default:
  //      this.role = 3;
  //      this.router.navigateByUrl(`/investor-reports/${this.userId}`);
  //      break;
  //  }
  //}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  //toggleAdmin() {
  //  this.showAdmin = !this.showAdmin;  
  //}
}


