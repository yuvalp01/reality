import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalAdminComponent } from './personal-admin.component';

describe('PersonalAdminComponent', () => {
  let component: PersonalAdminComponent;
  let fixture: ComponentFixture<PersonalAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonalAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
