import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordStrenghtComponent } from './password-strenght.component';

describe('PasswordStrenghtComponent', () => {
  let component: PasswordStrenghtComponent;
  let fixture: ComponentFixture<PasswordStrenghtComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PasswordStrenghtComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordStrenghtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
