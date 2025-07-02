import { Component, OnInit } from '@angular/core';
import { ICategories } from '../../models/Category';
import { IMenuItem } from '../../models/menuItem';
import { NgForm } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-update-item',
  templateUrl: './update-item.component.html',
  styleUrls: ['./update-item.component.css']
})
export class UpdateItemComponent implements OnInit {
  menuItemId: number = 0;
  categoryId: number = 0;
  ItemName: string = '';
  Price: number = 0;
  imageUrl: string = '';

  categories: ICategories[] = [];

  constructor(
    private _adminService: AdminService, private _router: Router, private _route: ActivatedRoute) {
  }

  ngOnInit() {

    this.menuItemId = this._route.snapshot.params['mId'];
    this.categoryId = this._route.snapshot.params['cId'];
    this.ItemName = this._route.snapshot.params['iName'];
    this.Price = this._route.snapshot.params['price'];
    this.imageUrl = this._route.snapshot.params['imgUrl'];

    if (this.menuItemId == 0 || this.menuItemId == null) {
      console.error('Invalid menu item ID');
      this._router.navigate(['/admin']);
    }
  }

  updateMenuItem(price: number) {
    this._adminService.UpdateMenuItem(this.menuItemId, price)
      .subscribe(
        (resSuccess) => {
          if (resSuccess) {
            alert("Menu Item Updated Successfully");
          }
          else {
            alert("Menu Item Update Failed");
          }
          this._router.navigate(['/manage-menu']);
        },
        (resError) => {
          console.log(resError);
          alert("Error occured while updating menu item");
        },
        () => { console.log("Menu item updated executed successfully!"); }
    )
  }
}
