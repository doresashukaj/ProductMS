import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './shared/auth.guard';
import { ProductsComponent } from './components/products/products.component';

export const routes: Routes = [
    {path:'', component: UserComponent,
        children: [
            {path: 'register',component:RegistrationComponent},
            {path:'login', component:LoginComponent},
            
        ]
    },
    {path:'dashboard', component: DashboardComponent,
        canActivate: [AuthGuard]
    },
    { path: 'products', component: ProductsComponent, canActivate: [AuthGuard] }
];
