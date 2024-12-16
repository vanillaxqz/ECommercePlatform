import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserResetPasswdComponent } from './user-reset-passwd.component';

describe('UserResetPasswdComponent', () => {
  let component: UserResetPasswdComponent;
  let fixture: ComponentFixture<UserResetPasswdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserResetPasswdComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserResetPasswdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
