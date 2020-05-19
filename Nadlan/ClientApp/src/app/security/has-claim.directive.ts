import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { SecurityService } from './security.service';
import { element } from 'protractor';

@Directive({
  selector: '[hasClaim]'
})
export class HasClaimDirective {

  constructor(private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef, private securityService:SecurityService
  ) {  }

  @Input() set hasClaim(claimType: any) {
    if (this.securityService.hasClaim(claimType)) {
      //Add template to DOM
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
    else {
      this.viewContainer.clear();
    }
  }

}
