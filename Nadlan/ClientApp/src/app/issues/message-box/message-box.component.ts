import { Component, OnInit, NgZone, ViewChild, Inject, ElementRef, Output, EventEmitter } from '@angular/core';
import { MessagesService } from '../meassages.service';
import { IMessage } from 'src/app/models';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { SecurityService } from 'src/app/security/security.service';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
  selector: 'message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css']
})
export class MessageBoxComponent implements OnInit {
  constructor(private _ngZone: NgZone,
    public dialogRef: MatDialogRef<MessageBoxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    private securityService: SecurityService,
    private messageService: MessagesService) { }
  messages: IMessage[];
  messageForm: FormGroup;
  newMessage: string;
  currentUser: string = 'unknown';
  tableName: string;
  parentId: number;
  @Output() readEmitter = new EventEmitter();
  @ViewChild('name', { static: true }) inputRef: ElementRef;
  ngOnInit() {
    this.messageForm = this.formBuilder.group({
      content: ['', Validators.required]
    });
    this.tableName = this.data.tableName;
    this.parentId = this.data.id;
    this.loadMessages();
    if (this.securityService.securityObject.userName != '') {
      let firstLetter = this.securityService.securityObject.userName.substr(0, 1).toUpperCase();
      this.currentUser = firstLetter + this.securityService.securityObject.userName.substr(1, 10);
    }

  }
  loadMessages() {
    this.messageService.getMessages(this.tableName, this.parentId).subscribe({
      next: result => {
        this.messages = result;
      },
      error: err => console.error(err)
    })
  }


  isUnreadByUser(message: IMessage) {
    let differentUser = message.userName != this.currentUser;
    let isUnread = !message.isRead;
    return isUnread && differentUser;
  }


  markAsRead(message: IMessage, formDirective: FormGroupDirective) {
    //formDirective.resetForm();
    message.isRead = true
    this.messageService.markAsRead(message).subscribe({
      next: result => {
        formDirective.resetForm();
        this.inputRef.nativeElement.focus();
        this.readEmitter.emit('read')
      },
      error: err => console.error(err)
    })

  }


  save(formDirective: FormGroupDirective) {
    if (this.messageForm.valid) {
      if (this.messageForm.dirty) {

        let message = {} as IMessage;
        message.parentId = this.parentId;
        message.tableName = this.tableName;
        message.content = this.messageForm.get('content').value;
        message.dateStemp = new Date();
        message.userName = this.currentUser;
        message.isRead = false;
        this.messageService.addNewMessage(message).subscribe({
          next: result => {
            this.loadMessages();
            formDirective.resetForm();
            this.messageForm.reset();
            this.readEmitter.emit('new')
            // this.messageForm.clearValidators();
            // this.messageForm.updateValueAndValidity();
          },
          error: err => console.error(err)
        });
      }
    }

  }



  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;
  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this._ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }

}


