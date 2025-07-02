import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { QRCodeModule } from 'angularx-qrcode';
import { ViewMenuItemsComponent } from './components/view-menu-items/view-menu-items.component';
import { LayoutComponent } from './components/layout/layout.component';
import { CustomerComponent } from './components/customer/customer.component';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { RaiseIssueComponent } from './components/raise-issue/raise-issue.component';
import { CustomerService } from './services/customer.service';
import { ViewOrdersComponent } from './components/view-orders/view-orders.component';
import { ProductsService } from './services/products.service';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AdminComponent } from './components/admin/admin.component';
import { SupportComponent } from './components/support/support.component';
import { RegisterComponent } from './components/register/register.component';
import { ViewcartComponent } from './components/viewcart/viewcart.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AddMenuComponent } from './components/add-menu/add-menu.component';
import { UpdateItemComponent } from './components/update-item/update-item.component';
import { ManageMenuComponent } from './components/manage-menu/manage-menu.component';
import { ViewIssuesComponent } from './components/view-issues/view-issues.component';
import { UpdateIssueComponent } from './components/update-issue/update-issue.component';
import { PersonalInfoComponent } from './components/personal-info/personal-info.component';
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component';
import { ChangepasswordComponent } from './components/changepassword/changepassword.component';
import { QrComponent } from './components/qr/qr.component';
import { BookTableComponent } from './components/book-table/book-table.component';
import { AdminService } from './services/admin.service';
import { SupportService } from './services/support.service';
import { UserService } from './services/user.service';
import { ConformationComponent } from './conformation/conformation.component';
import { ScannerComponent } from './components/scanner/scanner.component';
import { DeliveryDetailsComponent } from './components/delivery-details/delivery-details.component';
import { PaymentMethodComponent } from './components/payment-method/payment-method.component';
@NgModule({
  declarations: [
    AppComponent,
    ViewMenuItemsComponent,
    LayoutComponent,
    CustomerComponent,
    ViewOrdersComponent,
    UserInfoComponent,
    RaiseIssueComponent,
    HomeComponent,
    LoginComponent,
    AdminComponent,
    SupportComponent,
    RegisterComponent,
    ViewcartComponent,
    ProfileComponent,
    AddMenuComponent,
    UpdateItemComponent,
    ManageMenuComponent,
    ViewIssuesComponent,
    UpdateIssueComponent,
    PersonalInfoComponent,
    ForgetPasswordComponent,
    ChangepasswordComponent,
    QrComponent,
    BookTableComponent,
    ConformationComponent,
    ScannerComponent,
    DeliveryDetailsComponent,
    PaymentMethodComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    QRCodeModule,
   
  ],
  providers: [CustomerService, ProductsService, AdminService, SupportService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
