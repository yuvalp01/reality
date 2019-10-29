import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestorsOverviewComponent } from './investors-overview.component';

describe('InvestorsOverviewComponent', () => {
  let component: InvestorsOverviewComponent;
  let fixture: ComponentFixture<InvestorsOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvestorsOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestorsOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
