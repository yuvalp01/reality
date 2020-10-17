import { Component, OnInit, Input } from '@angular/core';
import { Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'drive-link-gen',
  templateUrl: './drive-link-gen.component.html',
  styleUrls: ['./drive-link-gen.component.css']
})
export class DriveLinkGenComponent implements OnInit {

  @Output() linkGeneratedEvent = new EventEmitter<string>();
  @Input()  originalLink: string = "";
 
  generatedLink: string;
  isShowInput:boolean = true;
  isSearching:boolean= false;
  constructor() { }

  ngOnInit() {
    //this.createDriveLink();
  }
  createDriveLink() {
    if (this.originalLink && !this.originalLink.includes('&id')) {
      this.isSearching = true;
      let start = this.originalLink.lastIndexOf('/d/') + 3;
      let secondPart = this.originalLink.substr(start, 1000);
      let end = secondPart.indexOf('/');
      let cleanId = secondPart.substr(0, end);
      this.generatedLink = `https://drive.google.com/uc?export=view&id=${cleanId}`
      this.isShowInput = false;
      this.isSearching = false;
      this.linkGeneratedEvent.emit(this.generatedLink);

    }

  }
}
