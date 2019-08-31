import { Component, OnInit, NgZone, ViewChild, Output, Inject } from "@angular/core";
import { ApartmentService } from "../services/apartment.service";
import { IApartment, ITransaction, IAccount } from "../models";
import { FormControl, FormGroup } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { TransactionService } from "../services/transaction.service";
import { Router } from "@angular/router";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import {take} from 'rxjs/operators';
import { EventEmitter } from "events";
import { MAT_DIALOG_DATA } from "@angular/material";


@Component(
{
templateUrl:'./add-transaction.component.html',
selector:'add-transaction'
})
export class AddTransactionComponent implements OnInit
{
    constructor (private apartmentService:ApartmentService, 
                 private accountService: AccountService,
                 private transactionService: TransactionService,
                 private router:Router,
                 private _ngZone: NgZone,
                // @Inject(MAT_DIALOG_DATA) public data: any
                 )
                 {

                 }
    apartments:IApartment[];
    accounts:IAccount[];
    transactionForm: FormGroup;

    //@Output() test = new EventEmitter();

    ngOnInit(): void {

      // console.log(this.data);

      this.apartmentService.getApartments().subscribe(result=> this.apartments = result,error=>console.error(error));
      this.accountService.getAccounts().subscribe(result=>this.accounts = result, error=>console.error(error));
    
        let apartment =  new FormControl(0);
        let account =  new FormControl(0);
        let amount =  new FormControl();
        let date =  new FormControl();
        let date_ =  new FormControl();
        let comments =  new FormControl();
        let isPurchaseCost =  new FormControl(false);
        this.transactionForm = new FormGroup(
            {
                date : date,
                date_ : date_,
                apartmentId: apartment,
                amount:amount,
                comments: comments,
                isPurchaseCost: isPurchaseCost,
                accountId:account

            });
    }
  saveTransaction(formValues: any): void{

      
    var transaction: ITransaction = Object.assign({}, formValues);
    ///fix UTC issue:
    let date = transaction.date;
    transaction.date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
    ///
        this.transactionService.addTransaction(transaction).subscribe(()=>{
            console.log("success!");
            this.router.navigate(['/fetch-transactions']);
        })
    }
    @ViewChild('autosize', {static: false}) autosize: CdkTextareaAutosize;
    triggerResize() {
      // Wait for changes to be applied, then trigger textarea resize.
      this._ngZone.onStable.pipe(take(1))
          .subscribe(() => this.autosize.resizeToFitContent(true)); 
    }

}
