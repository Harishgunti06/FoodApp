import { Injectable } from '@angular/core';
import { IMenuItem } from '../models/menuItem';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { ICategories } from '../models/Category';
import { IBooking } from '../models/Booking';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  menuItems: IMenuItem[] = [];
  categories: ICategories[] = [];
  constructor(private _http: HttpClient) { }

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  getAllMenuItems() {
    var tempVar = this._http.get<IMenuItem[]>("http://localhost:5214/api/Admin/GetAllMenuItems", { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    return tempVar;
  }

  getAllCategories() {
    var tempVar = this._http.get<ICategories[]>("http://localhost:5214/api/Customer/GetAllCategories", { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    return tempVar;
  }

  addMenuItemWithImage(menuItem: IMenuItem, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    // Step 1: Upload image
    return this._http.post<{ imageUrl: string }>('http://localhost:5214/api/Admin/upload-image', formData, { headers: this.getHeaders() }).pipe(
      switchMap(response => {
        menuItem.imageUrl = response.imageUrl;
        return this._http.post<IMenuItem[]>(
          'http://localhost:5214/api/Admin/AddMenuItem',
          menuItem,
          {
            responseType: 'text' as 'json',
            headers: this.getHeaders()
          }
        );
      }),
      catchError(this.errorHandler)
    );
  }


  UpdateMenuItem(menuItemId: number, price: number): Observable<boolean> {
    var menuItem: IMenuItem = {
      menuItemId: menuItemId,
      categoryId: 0,
      categoryName: '',
      itemName: '',
      price: price,
      imageUrl: '',
      isVegetarian: false,
      cuisineId: -1
    };
    let updatedItem = this._http.put<boolean>("http://localhost:5214/api/Admin/UpdateMenuItem", menuItem, { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    return updatedItem;
  }

  DeleteMenuItem(Item: IMenuItem) {
    const options = {
      body: Item,
      headers: this.getHeaders()
    };

    return this._http.delete<boolean>(
      "http://localhost:5214/api/Admin/DeleteMenuItem",
      options
    ).pipe(catchError(this.errorHandler));
  }


  ViewBookings() {
    var tempVar = this._http.get<IBooking[]>("http://localhost:5214/api/Customer/ViewBooking/view-booking", {headers: this.getHeaders()}).pipe(catchError(this.errorHandler));
    return tempVar;
  }

  checkIn(bookingId: number) {
    const url = `http://localhost:5214/api/Customer/CheckIn/${bookingId}`;
    return this._http.get<{ message: string }>(url, {headers: this.getHeaders()}).pipe(
      catchError(this.errorHandler)
    );
  }

  errorHandler(error: HttpErrorResponse) {
    console.error(error);
    return throwError(error.message || "Server Error");
  }
}
