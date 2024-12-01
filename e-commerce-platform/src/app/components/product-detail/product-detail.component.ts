import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit {
  product?: Product;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProductById(id).subscribe(product => {
      this.product = product;
    });
  }

  deleteProduct(): void {
    if (this.product && confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(Number(this.product.ProductId)).subscribe(() => {
        this.router.navigate(['/products']);
      });
    }
  }

  navigateToUpdate(): void {
    this.router.navigate(['/products/update', this.product?.ProductId]);
  }
}