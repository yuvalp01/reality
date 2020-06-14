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
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  role: number = 0;
  userId: number = 0;
  securityObject: AppUserAuth = null;
  constructor(private securityService: SecurityService,
    private router: Router) {
    this.securityObject = this.securityService.securityObject;
  }

  logout(): void {
    this.securityService.logout();
    this.router.navigateByUrl('login');
  }

  ngOnInit() {
    //if (this.securityObject.isAuthenticated) {
    //  //this.router.navigateByUrl(`/expenses`);
    //  var xx = this.securityObject;
    //}


  }

}


