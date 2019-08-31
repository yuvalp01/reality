import { Component, OnInit } from "@angular/core";
import { AccountService } from "../services/account.service";
import { IAccount } from "../models";



@Component({
    templateUrl: './add-account.component.html'
})
export class AddAccoutComponent implements OnInit
{
    constructor (private accountService:AccountService){}
    ngOnInit(): void {

    
    }
    saveAccount(formValues):void
    {
            let account_:IAccount  = Object.assign({},formValues);
            this.accountService.addAccount(account_).subscribe(()=>console.log('success!'));   
    }
}
