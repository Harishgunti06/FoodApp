import { Component } from '@angular/core';
import { ICategories } from '../../models/Category';
import { IMenuItem } from '../../models/menuItem';
import { AdminService } from '../../services/admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-menu',
  templateUrl: './manage-menu.component.html',
  styleUrls: ['./manage-menu.component.css']
})
export class ManageMenuComponent {
  categories: ICategories[] = [];
  menuItems: IMenuItem[] = [];
  selectedFile: File | null = null;
  filteredMenuItems: IMenuItem[] = [];
  showMessage: boolean = false;

  constructor(private _adminService: AdminService, private _router: Router) { }

  ngOnInit() {

    this._adminService.getAllCategories().subscribe(
      (resSuccess) => {
        this.categories = resSuccess;

        this._adminService.getAllMenuItems().subscribe(
          (resSuccess) => {
            this.menuItems = resSuccess.map(item => ({
              ...item,
              categoryName: this.categories.find(category => category.categoryId === item.categoryId)?.categoryName || 'Unknown'
            }));

            this.filteredMenuItems = this.menuItems;
            if (this.menuItems.length == 0) {
              this.showMessage = true;
            }
          },
          (resError) => {
            this.showMessage = true;
            this.menuItems = [];
            this.filteredMenuItems = [];
            console.log(resError);
          },
          () => { console.log("Menu items fetched and mapped successfully."); }
        )
      },
      (resError) => {
        this.showMessage = true;
        this.categories = [];
        console.log(resError);
      },
      () => { console.log("Get categories executed successfully"); }
    )
  }

  searchItemsByCategory(categoryId: string) {
    if (!categoryId) {
      this.filteredMenuItems = this.menuItems; // Show all if empty
      return;
    }

    this.filteredMenuItems = this.menuItems.filter(item =>
      item.categoryId.toString() === categoryId
    );
  }

  searchItemsByName(searchTerm: string) {
    this.filteredMenuItems = this.menuItems.filter(menuItem =>
      menuItem.itemName.toString().toLowerCase().includes(searchTerm.toLowerCase())
    );
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  AddNewMenuItem(menuItem: IMenuItem) {
    if (!this.selectedFile) {
      alert('Please select an image before adding the menu item.');
      return;
    }

    this._adminService.addMenuItemWithImage(menuItem, this.selectedFile).subscribe(
      (ressuccess) => {
        this.menuItems = ressuccess;
        this.filteredMenuItems = this.menuItems;
        alert("Menu Item Added Successfully");
        this.ngOnInit();  // reload data
      },
      (resError) => {
        this.showMessage = true;
        this.menuItems = [];
        this.filteredMenuItems = [];
        console.error(resError);
      },
      () => {
        console.log("Menu Item Added Successfully");
      }
    );
  }


  UpdateMenuItem(menuItem: IMenuItem) {
    this._router.navigate(['/update-item', menuItem.menuItemId, menuItem.categoryId, menuItem.itemName, menuItem.price, menuItem.imageUrl]);
  }

  DeleteMenuItem(Item: IMenuItem) {
    this._adminService.DeleteMenuItem(Item).subscribe(
      (resSuccess) => {
        if (resSuccess) {
          alert("Menu Item Deleted Successfully");
          this.ngOnInit();
        }
        else {
          alert("Menu Item is Already in the Customer Cart!!");
        }
        this._router.navigate(['/manage-menu']);
      },
      (resError) => {
        console.log(resError);
        alert("Can't delete the Menu Item");
      },
      () => { console.log("Delete Menu Item executed successfully"); }
    );
  }

  logout() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    this._router.navigate(['']);
  }
}
