import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ProductCreateComponent } from './product-create.component';
import { ProductService } from '../../services/product.service';

describe('ProductCreateComponent', () => {
  let component: ProductCreateComponent;
  let fixture: ComponentFixture<ProductCreateComponent>;
  let productService: jasmine.SpyObj<ProductService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const productServiceSpy = jasmine.createSpyObj('ProductService', ['createProduct']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, ProductCreateComponent],
      providers: [
        { provide: ProductService, useValue: productServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductCreateComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService) as jasmine.SpyObj<ProductService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a valid form when all fields are filled correctly', () => {
    component.productForm.setValue({
      name: 'Test Product',
      description: 'Test Description',
      price: '10.99',
      stock: '100',
      category: '1'
    });
    expect(component.productForm.valid).toBeTrue();
  });

  it('should have an invalid form when required fields are empty', () => {
    component.productForm.setValue({
      name: '',
      description: '',
      price: '',
      stock: '',
      category: ''
    });
    expect(component.productForm.invalid).toBeTrue();
  });

  it('should call createProduct and navigate on successful form submission', () => {
    component.productForm.setValue({
      name: 'Test Product',
      description: 'Test Description',
      price: '10.99',
      stock: '100',
      category: '1'
    });
    productService.createProduct.and.returnValue(of({}));

    component.onSubmit();

    expect(component.isSubmitting).toBeTrue();
    expect(productService.createProduct).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/products']);
  });

  it('should handle server errors on form submission', () => {
    component.productForm.setValue({
      name: 'Test Product',
      description: 'Test Description',
      price: '10.99',
      stock: '100',
      category: '1'
    });
    const errorResponse = {
      status: 400,
      error: { errors: { name: ['Name is required'] } }
    };
    productService.createProduct.and.returnValue(throwError(errorResponse));

    component.onSubmit();

    expect(component.isSubmitting).toBeFalse();
    expect(component.serverErrors).toEqual(['Name is required']);
  });

  it('should handle unexpected errors on form submission', () => {
    component.productForm.setValue({
      name: 'Test Product',
      description: 'Test Description',
      price: '10.99',
      stock: '100',
      category: '1'
    });
    productService.createProduct.and.returnValue(throwError({ status: 500 }));

    component.onSubmit();

    expect(component.isSubmitting).toBeFalse();
    expect(component.serverErrors).toEqual(['An unexpected error occurred']);
  });
});