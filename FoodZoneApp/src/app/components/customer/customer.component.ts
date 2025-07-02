import { Component } from '@angular/core';
import { ICategories } from '../../models/Category';
import { ProductsService } from '../../services/products.service';
import { Router } from '@angular/router';
import { IMenuItem } from '../../models/menuItem';
import { IMenuFilter } from '../../models/menuFilter';
import { CustomerService } from '../../services/customer.service';
import { ICuisine } from '../../models/cuisine';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent {
  foodCategories: ICategories[] = [];
  cuisines: ICuisine[] = [];
  message: string = "";
  showMsg: boolean = false;
  quantityMap: { [key: number]: number } = {};
  ratingsMap: { [key: number]: number } = {};
  showFilters = false;
  //filterSelections: { indian: boolean; chinese: boolean } = {
  //  indian: false,
  //  chinese: false
  //};

  filter: IMenuFilter = {
    cuisineIds: [],
    isVegetarian: null,
    minPrice: 0,
    maxPrice: 1000
  };

  filteredMenuItems: IMenuItem[] = [];
  filterSelections = { indian: false, chinese: false };
  //cuisines = [
  //  { id: 1, name: 'Indian' },
  //  { id: 2, name: 'Chinese' }
  //];
  constructor(private _productservice: ProductsService, private _customerservice: CustomerService, private _router: Router) {

  }

  ngOnInit() {
    this._productservice.getCategories().subscribe(
      (resSuccess) => {
        console.log("Categories received from API:", resSuccess);
        this.foodCategories = resSuccess;
      },
      (resError) => {
        this.foodCategories = [];
        console.log(resError);
      },
      () => {
        console.log("Category Displayed");
      }
    );
    this._customerservice.getAllCuisines().subscribe(
      (resSuccess) => {
        console.log("Cuisines received from API:", resSuccess);
        this.cuisines = resSuccess;
      },
      (resError) => {
        this.cuisines = [];
        console.log(resError);
      },
      () => {
        console.log("Cuisines displayed");
      }
    );
  }
  // In your component.ts
  categoryImages: { [key: string]: string } = {
    'Starters': 'assets/starters.jpg',
    'Main Course': 'assets/main-course.jpg',
    'Desserts': 'assets/desserts.jpg',
    'Beverages': 'assets/beverages.jpg'
  };

  getProductsByCategoryId(categoryId: number) {
    this._router.navigate(['/menu', categoryId]);  
  }

  //toggleFilter() {
  //  this.showFilter = !this.showFilter;
  //}

  updateCuisineIds(): void {
    const cuisineIds = [];
    if (this.filterSelections.indian) cuisineIds.push(1);
    if (this.filterSelections.chinese) cuisineIds.push(2);
    this.filter.cuisineIds = cuisineIds;
  }

  applyFilters(): void {
    //const cuisineIds = [];
    //if (this.filterSelections.indian) cuisineIds.push(1);    // Indian cuisineId = 1
    //if (this.filterSelections.chinese) cuisineIds.push(2);   // Chinese cuisineId = 2

    //const filterPayload = {
    //  cuisineIds,
    //  isVegetarian: this.filter.isVegetarian,
    //  minPrice: this.filter.minPrice,
    //  maxPrice: this.filter.maxPrice
    //};

    this._customerservice.filterMenuItems(this.filter).subscribe(
      (resSuccess) => {
        console.log("Filtered Menu Items:", resSuccess);
        this.filteredMenuItems = resSuccess;
      },
      (resError) => {
        this.filteredMenuItems = [];
        console.log(resError);
      },
      () => {
        console.log("Filter Applied");
      }
    );
  }

  clearFilters() : void {
    this.filteredMenuItems = [];
    this.filter = {
      cuisineIds: [],
      isVegetarian: null,
      minPrice: 0,
      maxPrice: 0
    };
    //this.filterSelections = [];
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

  goToCart() {
    this._router.navigate(['/view-cart']);
  }

  //bookTable() {
  //  this._router.navigate(['/book-table']);
  //}

  logout() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    this._router.navigate(['']);
  }
  
}
