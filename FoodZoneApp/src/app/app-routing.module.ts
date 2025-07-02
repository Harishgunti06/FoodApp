import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './components/customer/customer.component';
import { RaiseIssueComponent } from './components/raise-issue/raise-issue.component';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { ViewOrdersComponent } from './components/view-orders/view-orders.component';
import { ViewMenuItemsComponent } from './components/view-menu-items/view-menu-items.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SupportComponent } from './components/support/support.component';
import { AdminComponent } from './components/admin/admin.component';
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
import { ScannerComponent } from './components/scanner/scanner.component';
import { ConformationComponent } from './conformation/conformation.component';
import { DeliveryDetailsComponent } from './components/delivery-details/delivery-details.component';
import { PaymentMethodComponent } from './components/payment-method/payment-method.component';
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'customer', component: CustomerComponent },
  { path: 'raise-issue', component: RaiseIssueComponent },
  { path: 'user-info', component: UserInfoComponent },
  { path: 'view-orders', component: ViewOrdersComponent },
  { path: 'view-cart', component: ViewcartComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'support', component: SupportComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'changepassword', component: ChangepasswordComponent },
  { path: 'personal-info', component: PersonalInfoComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'manage-menu', component: ManageMenuComponent },
  { path: 'add-menu-item', component: AddMenuComponent },
  { path: 'book-table', component: BookTableComponent},
  {path: 'qr', component: QrComponent},
  { path: 'update-item/:mId/:cId/:iName/:price/:imgUrl', component: UpdateItemComponent },
  { path: 'update-issue/:iId/:iDesc/:oId/:email/:iStatus', component: UpdateIssueComponent },
  { path: 'menu/:categoryId', component: ViewMenuItemsComponent },
  { path: 'view-issues', component: ViewIssuesComponent },
  { path: 'qr/:bookingId', component: QrComponent },
  { path: 'booking', component: ConformationComponent },
  { path: 'delivery-details', component: DeliveryDetailsComponent },
  { path: 'paymentmethod', component: PaymentMethodComponent },
  { path: '', redirectTo: 'delivery', pathMatch: 'full' },
  { path: '**', component: HomeComponent } // Wildcard route for a 404 page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
