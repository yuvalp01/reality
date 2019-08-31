import {Component, OnInit} from '@angular/core';
import { IAccount } from '../models';
import { AccountService } from '../services/account.service';


@Component({
   templateUrl:'./fetch-accounts.component.html'
})
export class AccountListComponent implements OnInit
{
accounts : IAccount[];
constructor (private accountService: AccountService)
{

}
  ngOnInit():void
 {
  this.accountService.getAccounts().subscribe(result=>{this.accounts = result}, error=>console.error(error));
 }

}
