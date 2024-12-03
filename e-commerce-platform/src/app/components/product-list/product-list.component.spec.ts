import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ProductListComponent } from './product-list.component';
import { ProductService } from '../../services/product.service';
import { of, throwError } from 'rxjs';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let productService: ProductService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductListComponent, HttpClientTestingModule, RouterTestingModule],
      providers: [ProductService]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle error when loading products', () => {
    spyOn(productService, 'getProducts').and.returnValue(throwError('Error'));
    component.ngOnInit();
    expect(component.error).toBe('Error loading products');
    expect(component.isLoading).toBeFalse();
  });

  it('should calculate start and end index correctly', () => {
    component.currentPage = 2;
    component.pageSize = 10;
    component.totalItems = 25;
    expect(component.startIndex).toBe(11);
    expect(component.endIndex).toBe(20);
  });

  it('should load products on page change', () => {
    spyOn(component, 'loadProducts');
    component.totalPages = 3;
    component.onPageChange(2);
    expect(component.currentPage).toBe(2);
    expect(component.loadProducts).toHaveBeenCalled();
  });

  it('should not load products if page is out of range', () => {
    spyOn(component, 'loadProducts');
    component.totalPages = 3;
    component.onPageChange(4);
    expect(component.currentPage).not.toBe(4);
    expect(component.loadProducts).not.toHaveBeenCalled();
  });

  it('should return correct category name', () => {
    const categoryName = component.getCategoryName(1);
    expect(categoryName).toBe('Electronics');
  });

  it('should return "Unknown" for invalid category id', () => {
    const categoryName = component.getCategoryName(999);
    expect(categoryName).toBe('Unknown');
  });
});