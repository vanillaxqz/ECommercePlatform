// app.routes.ts
import { Routes } from '@angular/router';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserResetPasswdComponent } from './components/user-reset-passwd/user-reset-passwd.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component'
import { CartComponent } from './components/cart/cart.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrderConfirmationComponent } from './components/order-confirmation/order-confirmation.component';
import { AuthGuard } from './guards/auth.guard';

export const appRoutes: Routes = [
    { path: '', redirectTo: '/products', pathMatch: 'full' },
    { path: 'login', component: UserLoginComponent },
    { path: 'register', component: UserRegisterComponent },
    { path: 'reset-password', component: UserResetPasswdComponent },
    {
        path: 'products',
        component: ProductListComponent,
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
    },
    {
        path: 'profile',
        component: UserProfileComponent
    },
    {
        path: 'cart',
        component: CartComponent
    },
    {
        path: "checkout",
        component: CheckoutComponent
    },
    {
        path: 'order-confirmation/:id',
        component: OrderConfirmationComponent
    }
];