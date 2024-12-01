import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';

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

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) {
    this.productForm = this.fb.group({
      Name: ['', [Validators.required, Validators.maxLength(100)]],
      Description: ['', [Validators.required, Validators.maxLength(100)]],
      Price: ['', Validators.required],
      Stock: ['', Validators.required],
      Category: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id') || undefined;
    if (this.productId) {
      this.productService.getProductById(Number(this.productId)).subscribe(product => {
        this.productForm.patchValue(product);
      });
    }
  }

  onSubmit(): void {
    if (this.productForm.valid && this.productId) {
      const updatedProduct = {
        ...this.productForm.value,
        ProductId: this.productId
      };
      this.productService.updateProduct(updatedProduct).subscribe(() => {
        this.router.navigate(['/products']);
      });
    }
  }
}