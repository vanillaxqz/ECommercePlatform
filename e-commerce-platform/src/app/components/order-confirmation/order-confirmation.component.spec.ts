import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { OrderConfirmationComponent } from './order-confirmation.component';
import { CheckoutService } from '../../services/checkout.service';
import { OrderDetails } from '../../models/order.model';

describe('OrderConfirmationComponent', () => {
  let component: OrderConfirmationComponent;
  let fixture: ComponentFixture<OrderConfirmationComponent>;
  let checkoutService: jasmine.SpyObj<CheckoutService>;
  let router: jasmine.SpyObj<Router>;
  let activatedRoute: ActivatedRoute;

  beforeEach(async () => {
    const checkoutServiceSpy = jasmine.createSpyObj('CheckoutService', ['getOrderDetails']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const activatedRouteStub = {
      snapshot: {
        paramMap: {
          get: (key: string) => '12345'
        }
      }
    };

    await TestBed.configureTestingModule({
      declarations: [OrderConfirmationComponent],
      providers: [
        { provide: CheckoutService, useValue: checkoutServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: activatedRouteStub }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderConfirmationComponent);
    component = fixture.componentInstance;
    checkoutService = TestBed.inject(CheckoutService) as jasmine.SpyObj<CheckoutService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    activatedRoute = TestBed.inject(ActivatedRoute);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle order ID not found', () => {
    const activatedRouteStub = {
      snapshot: {
        paramMap: {
          get: (key: string) => null
        }
      }
    };
    TestBed.overrideProvider(ActivatedRoute, { useValue: activatedRouteStub });

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.error).toBe('Order ID not found');
    expect(component.isLoading).toBeFalse();
  });

  it('should have default steps', () => {
    expect(component.steps).toEqual([
      { label: 'Order Placed', complete: true },
      { label: 'Confirmed', complete: true },
      { label: 'Shipped', complete: false }
    ]);
  });
});