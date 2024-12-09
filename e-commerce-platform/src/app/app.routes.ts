// app.routes.ts
import { Routes } from '@angular/router';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { AuthGuard } from './guards/auth.guard';

export const appRoutes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: UserLoginComponent },
    { path: 'register', component: UserRegisterComponent },
    { 
        path: 'api/v1/products', 
        component: ProductListComponent,
        canActivate: [AuthGuard]
    },
    { 
        path: 'products/create', 
        component: ProductCreateComponent,
        canActivate: [AuthGuard]
    },
    { 
        path: 'products/:id', 
        component: ProductDetailComponent,
        canActivate: [AuthGuard]
    },
    { 
        path: 'products/update/:id', 
        component: ProductUpdateComponent,
        canActivate: [AuthGuard]
    }
];