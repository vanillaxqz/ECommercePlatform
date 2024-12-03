import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductDetailComponent } from './product-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Product } from '../../models/product.model';

describe('ProductDetailComponent', () => {
  let component: ProductDetailComponent;
  let fixture: ComponentFixture<ProductDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDetailComponent, RouterTestingModule, HttpClientTestingModule]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ProductDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should display error message', () => {
    component.error = 'Error loading product details';
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.error-message')?.textContent).toContain('Error loading product details');
  });

  it('should navigate to update product', () => {
    spyOn(component, 'navigateToUpdate');
    const button = fixture.nativeElement.querySelector('button.edit-btn');
    button.click();
    expect(component.navigateToUpdate).toHaveBeenCalled();
  });

  it('should delete product', () => {
    spyOn(component, 'deleteProduct');
    const button = fixture.nativeElement.querySelector('button.delete-btn');
    button.click();
    expect(component.deleteProduct).toHaveBeenCalled();
  });

  it('should navigate back to product list', () => {
    spyOn(component, 'goBack');
    const button = fixture.nativeElement.querySelector('button.back-btn');
    button.click();
    expect(component.goBack).toHaveBeenCalled();
  });
});