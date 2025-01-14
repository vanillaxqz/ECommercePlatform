import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { UserService } from '../../services/user.service';
import { Product } from '../../models/product.model';
import { NgOptimizedImage } from '@angular/common';

interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit {
  product?: Product;
  error?: string;
  categories = [
    'Electronics',
    'Fashion',
    'Garden',
    'HealthAndBeauty',
    'Sports',
    'Toys',
    'Games',
    'Books',
    'Jewelry'
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    public userService: UserService
  ) { }

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.loadProduct(productId);
    }
  }

  loadProduct(id: string): void {
    this.productService.getProductById(id).subscribe({
      next: (response: ApiResponse<Product>) => {
        if (response.isSuccess && response.data) {
          this.product = response.data;
          // console.log('IDs match?', this.userService.getCurrentUserId() === this.product.userId);
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

  getCategoryName(categoryId: number): string {
    return this.categories[categoryId - 1] || 'Unknown';
  }

  deleteProduct(): void {
    if (!this.product?.productId) return;

    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(this.product.productId).subscribe({
        next: () => {
          this.router.navigate(['/products']);
        },
        error: (err) => {
          this.error = 'Error deleting product';
          console.error('Error:', err);
        }
      });
    }
  }

  navigateToUpdate(): void {
    if (this.product?.productId) {
      this.router.navigate(['/products/update', this.product.productId]);
    }
  }

  goBack(): void {
    this.router.navigate(['/products']);
  }
}