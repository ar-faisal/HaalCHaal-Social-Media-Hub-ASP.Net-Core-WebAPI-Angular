import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ExploreComponent } from './user/explore/explore.component';
import { WritepostsComponent } from './user/writeposts/writeposts.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AccessdeniedComponent } from './accessdenied/accessdenied.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuardService } from './services/auth-guard.service';
import { GoogleComponent } from './google/google.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'Explore',
    component: ExploreComponent,
    canActivate: [AuthGuardService]

  },
  {
    path: 'Writeposts',
    component: WritepostsComponent,
    canActivate: [AuthGuardService]
  
  },
  
  {
    path: 'profile/:id',
    component: ProfileComponent,
    canActivate: [AuthGuardService]

  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  }
  ,
  {
    path: 'accessdenied',
    component: AccessdeniedComponent
  },
  {
    path: 'google',
    component: GoogleComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
