import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { UserService } from '../../services/user.service';
import { Product } from '../../models/product.model';
import { NgOptimizedImage } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { CartComponent } from '../cart/cart.component';

interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}

interface CartNotification {
  id: number;
  productName: string;
  timestamp: number;
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

  // Cart notification properties
  cartNotifications: CartNotification[] = [];
  private nextNotificationId = 0;
  private readonly MAX_NOTIFICATIONS = 4;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    public userService: UserService,
    private cartService: CartService
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

  // Add to Cart method
  addToCart(product: Product): void {
    CartComponent.addToCart(product);
    this.showCartNotification(product.name);
    this.cartService.updateCartCount();
  }

  // Show cart notification
  private showCartNotification(productName: string): void {
    const notification: CartNotification = {
      id: this.nextNotificationId++,
      productName,
      timestamp: Date.now()
    };
    
    this.cartNotifications = [notification, ...this.cartNotifications]
      .slice(0, this.MAX_NOTIFICATIONS);
    
    setTimeout(() => {
      this.cartNotifications = this.cartNotifications
        .filter(n => n.id !== notification.id);
    }, 3000);
  }
}