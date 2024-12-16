import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-password-strength',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="strength-meter">
      <div class="strength-text">Password Strength: {{ strengthText }}</div>
      <div class="strength-bars">
        <div class="bar" [ngClass]="getBarClass(0)"></div>
        <div class="bar" [ngClass]="getBarClass(1)"></div>
        <div class="bar" [ngClass]="getBarClass(2)"></div>
      </div>
      <div class="requirements-text" *ngFor="let req of unmetRequirements">
        {{ req }}
      </div>
    </div>
  `,
  styles: [`
    .strength-meter {
      margin-top: 0.5rem;
    }
    .strength-text {
      font-size: 0.75rem;
      color: #666;
      margin-bottom: 0.25rem;
    }
    .strength-bars {
      display: flex;
      gap: 4px;
      margin-bottom: 0.5rem;
    }
    .bar {
      height: 4px;
      flex: 1;
      background: #ddd;
      transition: background-color 0.3s;
    }
    .weak { background-color: #dc3545; }
    .medium { background-color: #ffc107; }
    .strong { background-color: #28a745; }
    .requirements-text {
      font-size: 0.75rem;
      color: #dc3545;
      margin-top: 0.25rem;
    }
  `]
})
export class PasswordStrengthComponent implements OnChanges {
  @Input() password: string = '';
  strengthText: string = '';
  unmetRequirements: string[] = [];

  ngOnChanges() {
    this.updateStrength();
  }

  private getRequirements() {
    return {
      length: this.password.length >= 8,
      uppercase: /[A-Z]/.test(this.password),
      lowercase: /[a-z]/.test(this.password),
      number: /[0-9]/.test(this.password),
      special: /[!@#$%^&*(),.?":{}|<>]/.test(this.password)
    };
  }

  private updateStrength() {
    const requirements = this.getRequirements();
    const metCount = Object.values(requirements).filter(Boolean).length;
    const allMet = Object.values(requirements).every(Boolean);

    // Update strength text and level
    if (allMet) {
      this.strengthText = 'Strong';
    } else if (metCount >= 3) {
      this.strengthText = 'Medium';
    } else {
      this.strengthText = 'Weak';
    }

    // Update unmet requirements messages
    this.unmetRequirements = [];
    if (!requirements.length) {
      this.unmetRequirements.push('Password must be at least 8 characters');
    }
    if (!requirements.number) {
      this.unmetRequirements.push('Password must contain at least one number');
    }
    if (!requirements.special) {
      this.unmetRequirements.push('Password must contain at least one special character (@, #, $, etc.)');
    }
    if (!requirements.uppercase) {
      this.unmetRequirements.push('Password must contain at least one uppercase letter');
    }
    if (!requirements.lowercase) {
      this.unmetRequirements.push('Password must contain at least one lowercase letter');
    }
  }

  getBarClass(index: number): string {
    const requirements = this.getRequirements();
    const metCount = Object.values(requirements).filter(Boolean).length;
    const allMet = Object.values(requirements).every(Boolean);

    if (allMet) {
      return index <= 2 ? 'strong' : '';
    } else if (metCount >= 3) {
      return index <= 1 ? 'medium' : '';
    } else {
      return index === 0 ? 'weak' : '';
    }
  }
}