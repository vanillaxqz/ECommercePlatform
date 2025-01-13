import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PasswordStrengthComponent } from './password-strenght.component';
import { By } from '@angular/platform-browser';

describe('PasswordStrengthComponent', () => {
  let component: PasswordStrengthComponent;
  let fixture: ComponentFixture<PasswordStrengthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PasswordStrengthComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordStrengthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display password strength text', () => {
    component.strengthText = 'Weak';
    fixture.detectChanges();
    const strengthText = fixture.debugElement.query(By.css('.strength-text')).nativeElement;
    expect(strengthText.textContent).toContain('Password Strength: Weak');
  });

  it('should display strength bars with correct classes', () => {
    component.strengthText = 'Weak';
    fixture.detectChanges();
    const bars = fixture.debugElement.queryAll(By.css('.bar'));
    expect(bars.length).toBe(3);
    expect(bars[0].classes['weak']).toBeTruthy();
    expect(bars[1].classes['medium']).toBeFalsy();
    expect(bars[2].classes['strong']).toBeFalsy();
  });

  it('should display unmet requirements', () => {
    component.unmetRequirements = ['At least 8 characters', 'At least 1 uppercase letter'];
    fixture.detectChanges();
    const requirements = fixture.debugElement.queryAll(By.css('.requirements-text'));
    expect(requirements.length).toBe(2);
    expect(requirements[0].nativeElement.textContent).toContain('At least 8 characters');
    expect(requirements[1].nativeElement.textContent).toContain('At least 1 uppercase letter');
  });

  it('should get correct bar class', () => {
    component.strengthText = 'Weak';
    expect(component.getBarClass(0)).toBe('weak');
    expect(component.getBarClass(1)).toBe('');
    expect(component.getBarClass(2)).toBe('');

    component.strengthText = 'Medium';
    expect(component.getBarClass(0)).toBe('medium');
    expect(component.getBarClass(1)).toBe('medium');
    expect(component.getBarClass(2)).toBe('');

    component.strengthText = 'Strong';
    expect(component.getBarClass(0)).toBe('strong');
    expect(component.getBarClass(1)).toBe('strong');
    expect(component.getBarClass(2)).toBe('strong');
  });
});