import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-create',
  standalone  : true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent {
  productForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private ProductService: ProductService,
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

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.productForm.valid) {
      this.ProductService.createProduct(this.productForm.value).subscribe(() => {
        this.router.navigate(['/books']);
      });
    }
  }


}
