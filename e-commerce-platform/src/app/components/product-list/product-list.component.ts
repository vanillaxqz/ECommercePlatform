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

  constructor(private productService: ProductService, private router: Router) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts(this.currentPage, this.pageSize)
      .subscribe((result) => {
        this.products = result.data.map(item => ({
          productId: item.productId, // Map camelCase to PascalCase
          name: item.name,
          description: item.description,
          price: item.price,
          stock: item.stock,
          category: item.category
        }));
        console.log('Mapped Products:', this.products); // Verify the mapping
      });
  }



  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProducts();
  }

  navigateToCreate() {
    this.router.navigate(['products/create']);
  }

  navigateToDetail(id: string | undefined) {
    this.router.navigate(['/products', id]);
  }

  navigateToUpdate(id: string | undefined) {
    this.router.navigate(['/products/update', id]);
  }
}