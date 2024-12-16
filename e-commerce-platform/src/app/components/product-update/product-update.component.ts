import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-update',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-update.component.html',
  styleUrl: './product-update.component.css'
})
export class ProductUpdateComponent implements OnInit {
  productForm: FormGroup;
  productId?: string;
  product?: Product;
  error?: string;
  isSubmitting = false;
  predictedPrice?: number;
  isPredicting = false;

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
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      price: ['', [Validators.required, Validators.min(0), Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      stock: ['', [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
      category: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id') ?? undefined;
    if (this.productId) {
      this.loadProduct(this.productId);
    }
  }

  predictPrice(): void {
    if (!this.productForm.get('name')?.valid || !this.productForm.get('description')?.valid) return;
    
    this.isPredicting = true;
    const formData = {
      ...this.productForm.value,
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

  loadProduct(id: string): void {
    this.productService.getProductById(id).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.product = response.data;
          this.productForm.patchValue({
            name: response.data.name,
            description: response.data.description,
            price: response.data.price,
            stock: response.data.stock,
            category: response.data.category
          });
        } else {
          this.error = response.errorMessage || 'Failed to load product';
        }
      },
      error: (err) => {
        this.error = 'Error loading product details';
        console.error('Error:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.valid && this.productId) {
      this.isSubmitting = true;
      const updatedProduct = {
        productId: this.productId,
        ...this.productForm.value,
        category: parseInt(this.productForm.value.category, 10)
      };

      this.productService.updateProduct(updatedProduct).subscribe({
        next: () => {
          this.router.navigate(['/products']);
        },
        error: (err) => {
          this.isSubmitting = false;
          this.error = 'Error updating product';
          console.error('Error:', err);
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/products']);
  }
}