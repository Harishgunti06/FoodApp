import { Component, OnInit } from '@angular/core';
import { IMenuItem } from '../../models/menuItem';
import { AdminService } from '../../services/admin.service';
import { NgForm } from '@angular/forms';
import { ICategories } from '../../models/Category';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-menu',
  templateUrl: './add-menu.component.html',
  styleUrls: ['./add-menu.component.css']
})
export class AddMenuComponent implements OnInit {
  categories: ICategories[] = [];
  file: File | null = null;
  menuItem: IMenuItem = {
    menuItemId: 0,
    categoryId: -1,
    categoryName: '',
    itemName: '',
    price: 0,
    imageUrl: '',
    isVegetarian: false,
    cuisineId:-1
  
    //description: ''
  };
  constructor(private _adminService: AdminService, private _router: Router) { }

  onFileSelected(event: any) {
    const fileInput = event.target.files[0];
    if (fileInput) {
      this.file = fileInput;
    }
  }

  addMenuItem(form: NgForm) {
    if (form.valid && this.file) {
      this._adminService.addMenuItemWithImage(this.menuItem, this.file).subscribe(
        (response) => {
          alert("Menu Item Added Successfully");
          this._router.navigate(['/manage-menu']);
        },
        (error) => {
          console.error('Error adding menu item with image', error);
        }
      );
    } else {
      console.error('Form is invalid or image file is missing');
      alert('Please fill all fields and select an image.');
    }
  }


  onCategoryChange(event: any) {
    const selectedCategory = this.categories.find(c => c.categoryId === +event.target.value);
    if (selectedCategory) {
      this.menuItem.categoryId = selectedCategory.categoryId;
      this.menuItem.categoryName = selectedCategory.categoryName;
    }
  }

  ngOnInit() {
    this._adminService.getAllCategories().subscribe(
      (resSuccess) => {
        this.categories = resSuccess;
      },
      (resError) => {
        this.categories = [];
        console.log(resError);
      },
      () => { console.log("Get All categories executed"); }
    )
  }
}
