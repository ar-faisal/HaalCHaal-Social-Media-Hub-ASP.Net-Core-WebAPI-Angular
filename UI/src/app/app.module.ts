import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { WritepostsComponent } from './user/writeposts/writeposts.component';
import { AccessdeniedComponent } from './accessdenied/accessdenied.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DeleteusersComponent } from './admin/deleteusers/deleteusers.component';
import { SsinterceptorService } from './services/ssinterceptor.service';
import { ExploreComponent } from './user/explore/explore.component';
import { ProfileComponent } from './profile/profile.component';
import { GoogleComponent } from './google/google.component';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { LoadingBarModule } from '@ngx-loading-bar/core';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    WritepostsComponent,
    AccessdeniedComponent,
    DeleteusersComponent,
    ExploreComponent,
    ProfileComponent,
    GoogleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule, 
    LoadingBarHttpClientModule,
    LoadingBarRouterModule,
    LoadingBarModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS, useClass:
      SsinterceptorService, multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
