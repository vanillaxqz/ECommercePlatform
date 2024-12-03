import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductUpdateComponent } from './product-update.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { of, throwError } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

describe('ProductUpdateComponent', () => {
  let component: ProductUpdateComponent;
  let fixture: ComponentFixture<ProductUpdateComponent>;
  let productService: ProductService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductUpdateComponent, RouterTestingModule, HttpClientTestingModule, ReactiveFormsModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '1' // Mock product ID
              }
            }
          }
        }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ProductUpdateComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load product details on init', () => {
    const testProduct = {
      productId: '1',
      name: 'Test Product',
      description: 'Test Description',
      price: 100,
      stock: 10,
      category: 1
    };
    spyOn(productService, 'getProductById').and.returnValue(of({ isSuccess: true, data: testProduct }));
    component.ngOnInit();
    expect(component.product).toEqual(testProduct);
    expect(component.productForm.value.name).toEqual(testProduct.name);
  });

  it('should display error message if product loading fails', () => {
    spyOn(productService, 'getProductById').and.returnValue(throwError('Error loading product details'));
    component.ngOnInit();
    expect(component.error).toBe('Error loading product details');
  });

  it('should navigate back to product list on cancel', () => {
    spyOn(component, 'goBack');
    const button = fixture.nativeElement.querySelector('button.cancel-button');
    button.click();
    expect(component.goBack).toHaveBeenCalled();
  });

  it('should submit the form successfully', () => {
    const testProduct = {
      productId: '1',
      name: 'Test Product',
      description: 'Test Description',
      price: 100,
      stock: 10,
      category: 1
    };
    spyOn(productService, 'updateProduct').and.returnValue(of({ isSuccess: true }));
    component.productForm.setValue({
      name: testProduct.name,
      description: testProduct.description,
      price: testProduct.price,
      stock: testProduct.stock,
      category: testProduct.category.toString()
    });
    component.onSubmit();
    expect(component.isSubmitting).toBeTrue();
  });
});