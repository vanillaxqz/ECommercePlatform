import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent {
  productForm: FormGroup;
  isSubmitting = false;
  serverErrors: string[] = [];
  predictedPrice?: number;
  isPredicting = false;
  predictionError?: string;

  categories = [
    { id: 1, name: 'Electronics' },
    { id: 2, name: 'Fashion' },
    { id: 3, name: 'Garden' },
    { id: 4, name: 'HealthAndBeauty' },
    { id: 5, name: 'Sports' },
    { id: 6, name: 'Toys' },
    { id: 7, name: 'Games' },
    { id: 8, name: 'Books' },
    { id: 9, name: 'Jewelry' }
  ];

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      price: [0, [Validators.required, Validators.min(0), Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      stock: ['', [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
      category: ['', Validators.required]
    });
  }

  predictPrice(): void {
    const name = this.productForm.get('name')?.value;
    const description = this.productForm.get('description')?.value;
    const category = this.productForm.get('category')?.value;
    
    if (!name || !description || !category) {
      this.predictionError = 'Please provide name, description and category';
      return;
    }
    
    this.predictionError = undefined;
    this.isPredicting = true;
    
    const formData = {
      ...this.productForm.value,
      category: parseInt(this.productForm.value.category, 10),
      price: 0
    };

    this.productService.predictPrice(formData).subscribe({
      next: (response) => {
        this.predictedPrice = response;
        this.productForm.patchValue({ price: response });
        this.isPredicting = false;
      },
      error: (err) => {
        console.error('Error:', err);
        this.isPredicting = false;
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.isSubmitting = true;
      const formData = {
        ...this.productForm.value,
        category: parseInt(this.productForm.value.category, 10)
      };

      this.productService.createProduct(formData).subscribe({
        next: () => {
          this.router.navigate(['/products']);
        },
        error: (error) => {
          this.isSubmitting = false;
          if (error.status === 400 && error.error && error.error.errors) {
            this.serverErrors = Object.values(error.error.errors).flat() as string[];
          } else {
            this.serverErrors = ['An unexpected error occurred'];
          }
        }
      });
    }
  }
}