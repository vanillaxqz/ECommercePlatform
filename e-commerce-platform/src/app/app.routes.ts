import { Routes } from '@angular/router';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';

export const appRoutes: Routes = [
    {path: '', redirectTo: '/products', pathMatch: 'full' },
    {path: 'products', component: ProductListComponent},
    {path: 'products/create', component: ProductCreateComponent},
    {path: 'products/:id', component: ProductDetailComponent},
    {path: 'products/update/:id', component: ProductUpdateComponent}
];
