import { Component } from '@angular/core';
import { IOrderItem } from '../../models/orderitems';
import { CustomerService } from '../../services/customer.service';
import { Router } from '@angular/router';
import { IRating } from '../../models/ratings';

@Component({
  selector: 'app-view-orders',
  templateUrl: './view-orders.component.html',
  styleUrls: ['./view-orders.component.css']
})
export class ViewOrdersComponent {
  orders: IOrderItem[] = [];
  ratingMap: { [menuItemId: number]: number } = {};
  ratedItems: Set<number> = new Set();
  email: string = sessionStorage.getItem('userName') || ''; 
  constructor(private _customerservice: CustomerService, private _router: Router) {

  }
  ngOnInit() {
    this._customerservice.getRatedItemsForUser(this.email).subscribe(
      (resSuccess) => {
        this.ratedItems = new Set(resSuccess.map(item => item.orderItemId));
      },
      (resError) => {
        console.log(resError);
      },
      () => {
        console.log("Rated Items Retrieved");
      }
    );

    this._customerservice.getOrderItems().subscribe(
      (resSuccess) => {
        this.orders = resSuccess;
      },
      (resError) => {
        this.orders = [];
        console.log(resError);
      },
      () => {
        console.log("Category Displayed");
      }
    )
  }
 
  setRating(menuItemId: number, ratingValue: number, orderItemId: number): void {
    const rating: IRating = {
      menuItemId: menuItemId,
      itemName: this.orders.find(item => item.menuItemId === menuItemId)?.itemName || '',
      orderItemId: orderItemId,
      ratingValue: ratingValue,
      email: sessionStorage.getItem('userName') || '' 
    };

    this._customerservice.SetRating(rating).subscribe(
      (resSuccess) => {
        this.ratingMap[orderItemId] = ratingValue;
        this.ratedItems.add(orderItemId);
      },
      (resError) => {
        console.log(resError);
      }
    );
  }


  goBack() {
    this._router.navigate(["/customer"]);
  }
}
