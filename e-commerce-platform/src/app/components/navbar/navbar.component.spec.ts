import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NavbarComponent } from './navbar.component';
import { CartService } from '../../services/cart.service';
import { of } from 'rxjs';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let cartService: jasmine.SpyObj<CartService>;

  beforeEach(async () => {
    const cartServiceSpy = jasmine.createSpyObj('CartService', ['getCartCount']);
    cartServiceSpy.getCartCount.and.returnValue(of(5));

    await TestBed.configureTestingModule({
      declarations: [NavbarComponent],
      imports: [RouterTestingModule],
      providers: [
        { provide: CartService, useValue: cartServiceSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render navigation links', () => {
    const compiled = fixture.nativeElement;
    const navLinks = compiled.querySelectorAll('a.nav-link');
    expect(navLinks.length).toBeGreaterThan(0);
  });

  it('should highlight the active link', () => {
    const compiled = fixture.nativeElement;
    const activeLink = compiled.querySelector('a.nav-link.active');
    expect(activeLink).toBeTruthy();
  });

  it('should display the cart count', () => {
    const compiled = fixture.nativeElement;
    const cartCount = compiled.querySelector('.cart-count');
    expect(cartCount.textContent).toContain('5');
  });
});