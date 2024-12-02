import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  products: Product[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

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

  constructor(private productService: ProductService, private router: Router) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts(this.currentPage, this.pageSize)
      .subscribe({
        next: (result) => {
          if (result.isSuccess && result.data) {
            this.products = result.data;
          }
        },
        error: (error) => {
          console.error('Error loading products:', error);
        }
      });
  }

  getCategoryName(categoryId: number): string {
    return this.categories[categoryId - 1] || 'Unknown';
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProducts();
  }

  navigateToCreate(): void {
    this.router.navigate(['/products/create']);
  }

  navigateToDetail(id: string): void {
    this.router.navigate(['/products', id]);
  }

  navigateToUpdate(id: string): void {
    this.router.navigate(['/products/update', id]);
  }
}