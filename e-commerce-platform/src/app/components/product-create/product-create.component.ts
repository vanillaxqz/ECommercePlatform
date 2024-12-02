import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-create',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.css']
})
export class ProductCreateComponent {
  productForm: FormGroup;
  serverErrors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router
  ) {
    this.productForm = this.fb.group({
      Name: ['', [Validators.required, Validators.maxLength(100)]],
      Description: ['', [Validators.required, Validators.maxLength(100)]],
      Price: ['', Validators.required],
      Stock: ['', Validators.required],
      Category: ['', Validators.required]
    });
  }

  ngOnInit(): void { }

  onSubmit(): void {
    if (this.productForm.valid) {
      const formData = { ...this.productForm.value, Category: parseInt(this.productForm.value.Category, 10) };
      this.productService.createProduct(formData).subscribe({
        next: () => {
          this.router.navigate(['/products']);
        },
        error: (error) => {
          if (error.status === 400 && error.error && error.error.errors) {
            this.serverErrors = Object.values(error.error.errors).flat() as string[];
          } else {
            console.error('Unexpected error:', error);
          }
        }
      });
    }
  }
}