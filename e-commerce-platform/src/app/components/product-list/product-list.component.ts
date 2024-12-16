import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { UserService } from '../../services/user.service';
import { NgOptimizedImage } from '@angular/common';

interface Category {
  id: number;
  name: string;
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule, NgOptimizedImage],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  currentPage = 1;
  pageSize = 9;
  totalItems = 0;
  totalPages = 0;
  isLoading = false;
  error?: string;
  selectedCategory?: number;

  filters = {
    name: '',
    stock: undefined as number | undefined,
    price: undefined as number | undefined
  };

  categories: Category[] = [
    { id: 1, name: 'Electronics' },
    { id: 2, name: 'Fashion' },
    { id: 3, name: 'Garden' },
    { id: 4, name: 'HealthAndBeauty' },
    { id: 5, name: 'Sports' },
    { id: 6, name: 'Toys' },
    { id: 7, name: 'Games' },
    { id: 8, name: 'Books' },
    { id: 9, name: 'Jewelry' },
  ];

  constructor(private productService: ProductService, private router: Router, public userService: UserService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading = true;
    this.error = undefined;
  
    this.productService
      .getProducts(this.currentPage, this.pageSize, this.selectedCategory, this.filters.name, this.filters.stock, this.filters.price)
      .subscribe({
        next: (response) => {
          this.products = response.data;
          this.totalItems = response.totalCount;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.isLoading = false;
        },
        error: (error) => {
          this.isLoading = false;
          
          if (error.status === 401 && !this.userService.isAuthenticated) {
            this.error = 'Please log in to access all features. Some functionality may be limited for guests';
          } else if (error.status === 401) {
            this.error = 'Your session has expired. Please log in again';
          } else if (error.status === 403) {
            this.error = 'You do not have permission to view these products';
          } else if (error.status === 0) {
            this.error = 'Unable to connect to server. Please check your internet connection';
          } else if (error.status === 404) {
            this.error = 'No products found';
          } else {
            this.error = 'Unable to load products. Please try again later';
          }
          console.error('Error loading products:', error);
        }
      });
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onCategoryChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.selectedCategory = select.value ? Number(select.value) : undefined;
    this.currentPage = 1; // Reset to first page when category changes
    this.loadProducts();
  }

  getCategoryName(categoryId: number): string {
    const category = this.categories.find((c) => c.id === categoryId);
    return category ? category.name : 'Unknown';
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.loadProducts();
    }
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

  get startIndex(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get endIndex(): number {
    const end = this.currentPage * this.pageSize;
    return Math.min(end, this.totalItems);
  }
}
