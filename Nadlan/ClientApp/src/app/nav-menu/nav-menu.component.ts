import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  userType: number = 0;
  userId: number = 0;
  //showAdmin: boolean = false;
  constructor(private route:ActivatedRoute, private router: Router) { }

  ngOnInit() {
    //let xxx = this.route.snapshot.queryParams['usertype'];
    //const queryParams = this.route.snapshot.queryParams;
    //let yyy = queryParams.usertype;
    ////this.route.queryParamMap.subscribe(params => {
    ////  this.userType =  +params.get('usertype');
    ////})
    //this.userType = 1;
    this.userId = 199;
    this.configurePage(this.userId);
    window.sessionStorage.setItem("userType", this.userType.toString());
    //if (this.userType==1) {
    //  console.log('xxxxx');
    //}
  }

  configurePage(userId:number) {
    switch (userId) {
      case 199:
        this.userType = 1;
        this.router.navigateByUrl(`/fetch-transactions`);
        break;
      case 107:
        this.userType = 2;
        this.router.navigateByUrl(`/expenses`);
        break;
      default:
        this.userType = 3;
        this.router.navigateByUrl(`/investor-reports/${this.userId}`);
        break;
    }
  }

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
