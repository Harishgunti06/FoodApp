import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  private totalAmount: number = 0;
  private cartItems: any[] = [];
  setTotalAmount(amount: number) {
    this.totalAmount = amount;
  }

  getTotalAmount(): number {
    return this.totalAmount;
  }
  setCartItems(items: any[]) {
    this.cartItems = items;
  }
  getCartItems(): any[] {
    return this.cartItems;
  }
}
