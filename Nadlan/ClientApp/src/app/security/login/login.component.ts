import { Component, OnInit } from '@angular/core';
import { AppUser } from '../app.user';
import { AppUserAuth } from '../app.user.auth';
import { SecurityService } from '../security.service';
import { ActivatedRoute, Router } from '@angular/router';
import { element } from 'protractor';


@Component({
  selector: 'ptc-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user: AppUser = new AppUser();
  securityObject: AppUserAuth = null;
  returnUrl: string;

  constructor(private securityService: SecurityService,
    private route: ActivatedRoute,
    private router: Router) { }

  login() {
    this.securityService.login(this.user).subscribe(
      resp => {
        this.securityObject = resp;
        if (this.returnUrl) {
          this.router.navigateByUrl(this.returnUrl);
        }
        else {
          this.navigateByUser(this.securityObject.userName.toLowerCase());
        }
      },
      () => {
        this.securityObject = new AppUserAuth();
      }
    );
  }
  navigateByUser(userName:string) {
    switch (userName) { 
      case 'yuval':
        this.router.navigateByUrl(`/products`);
        break;
      case 'stella':
        this.router.navigateByUrl(`/issue-list`);
        break;
      case 'ohad':
        this.router.navigateByUrl(`/investor-reports/101`);
        break;
      case 'avi':
        this.router.navigateByUrl(`/investor-reports/102`);
        break;
      case 'etai':
        this.router.navigateByUrl(`/investor-reports/103`);
        break;
      case 'naphtali':
        this.router.navigateByUrl(`/investor-reports/104`);
        break;
      default:
        //this.router.navigateByUrl(`/investor-reports/101`);
        break;
    }
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
    //if (this.securityObject.isAuthenticated) {

    //  this.router.navigateByUrl(`/expenses`);

    //}
  }

}
