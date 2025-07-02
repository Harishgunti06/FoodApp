import { Component, OnInit } from '@angular/core';
import { IMenuItem } from '../../models/menuItem';
import { ProductsService } from '../../services/products.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../services/customer.service';

@Component({
  selector: 'app-view-menu-items',
  templateUrl: './view-menu-items.component.html',
  styleUrls: ['./view-menu-items.component.css']
})
export class ViewMenuItemsComponent  {
  menuItems: IMenuItem[] = [];
  message: string = "";
  showMsg: boolean = false;
  categoryId: number = 0;
  quantity: number = 0;
  quantityMap: { [menuItemId: number]: number } = {};
  ratingsMap: { [key: number]: number } = {};
  //Implement necessary logic
  constructor(private _productservice: ProductsService, private route: ActivatedRoute, private _customerservice: CustomerService, private _router: Router) {
    this.categoryId = this.route.snapshot.params['categoryId'];
  }

  ngOnInit() {
    
    this._productservice.getProductsByCategoryId(this.categoryId).subscribe(
      (resSuccess) => {
        this.menuItems = resSuccess;
        this.loadRatings();
      },
      (resError) => {
        this.menuItems = [];
        console.log(resError);
      },
      () => {
        console.log("Category Displayed");
      }
    );   
  }
  
  loadRatings(): void {
    this.menuItems.forEach(item => {
      this._customerservice.displayRatings(item.menuItemId).subscribe(rating => {
        this.ratingsMap[item.menuItemId] = rating;
      });
    });
  }

  addItemToCart(item: IMenuItem, quantity: number) {
    if (!quantity || quantity < 1) {
      alert('Please enter a valid quantity.');
      return;
    }

    this._customerservice.addToCart(item, quantity).subscribe(
      (resSuccess) => {
        alert('Item added to cart successfully!');
        this.message = '';
        this.showMsg = false;
      },
      (resError) => {
        this.message = 'Failed to add item to cart.';
        this.showMsg = true;
        console.error(resError);
      },
      () => {
        console.log('Add to cart request completed.');
      }
    );
  }

  displayRatings(menuItemId: number) {
    this._customerservice.displayRatings(menuItemId).subscribe(
      (resSuccess) => {
        // Handle success response, e.g., display ratings in a modal or alert
        console.log('Ratings:', resSuccess);

        return resSuccess;
      },
      (resError) => {
        // Handle error response
        console.error('Error fetching ratings:', resError);
      },
      () => {
        console.log('Ratings fetch request completed.');
      }

    )
  }

  goToCart() {
    this._router.navigate(['/view-cart']);
  }

  goBack() {
    this._router.navigate(['/customer']);
   }
}
