import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-payment-method',
  templateUrl: './payment-method.component.html',
  styleUrls: ['./payment-method.component.css']
})
export class PaymentMethodComponent {
  totalAmount: number = 0;
  paymentMethod: string = 'card';  // No method selected initially

  email: string = '';
  upiId: string = '';
  cardMounted: boolean = false;
  private cartItems: any[] = [];
  constructor(private _sharedService: SharedService, private _customerService: CustomerService, private _router: Router) { }

  async ngOnInit(): Promise<void> {
    this.totalAmount = this._sharedService.getTotalAmount();
  }
  placeOrder(): void {
    const email = sessionStorage.getItem('userName') || '';
    this.cartItems = this._sharedService.getCartItems();
    console.log(this.cartItems);
    const orderItems = this.cartItems.map(item => ({
      orderItemId: 0,
      email: email,
      menuItemId: item.menuItemId,
      price: item.price,
      itemName: item.itemName,
      quantity: item.quantity,
    }));
    this._customerService.AddToOrder(orderItems).subscribe(
      (resSuccess) => {
        alert('Order Placed Successfully');
        this._router.navigate(['/view-orders']);
      },
      (resError) => {
        console.error(resError);
      },
      () => {
        console.log('Order placement request completed.');
      }
    );
  }
}
