import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { IIssue } from '../models/issue';
import { IOrderItem } from '../models/orderitems';
import { IMenuItem } from '../models/menuItem';
import { ICart } from '../models/cart';
import { IUser } from '../models/user';
import { IRating } from '../models/ratings';
import { IMenuFilter } from '../models/menuFilter';
import { ICuisine } from '../models/cuisine';
import { IBooking } from '../models/Booking';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  email: string = "";
  order: IOrderItem[] = [];
  constructor(private _http: HttpClient) {
  }

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  RaiseIssue(orderItemId: number, IssueDescription: string) {
    /*  this.email = sessionStorage.getItem('userName') || '';*/
    this.email = sessionStorage.getItem('userName') || '';
    const params = new HttpParams()

      .set('email', this.email)
      .set('orderItemId', orderItemId.toString())
      .set('description', IssueDescription);

    const url = 'http://localhost:5214/api/Customer/RaiseIssue';

    return this._http.post(url, null, { params, responseType: 'text', headers: this.getHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getOrderItems() {
    this.email = sessionStorage.getItem('userName') || '';
    const params = new HttpParams()
      .set('email', this.email)

    return this._http.get<IOrderItem[]>('http://localhost:5214/api/Customer/GetOrdersByCustomer', { params, headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
  }

  getAllCuisines() {
    return this._http.get<ICuisine[]>('http://localhost:5214/api/Customer/GetAllCuisines/getallcuisines', { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
  }

  filterMenuItems(filter: IMenuFilter) {
    return this._http.post<IMenuItem[]>('http://localhost:5214/api/Customer/FilterMenuItems/filter-menu', filter, { headers: this.getHeaders() })
  }

  addToCart(item: IMenuItem, quantity: number) {
    const email = sessionStorage.getItem('userName') || '';
    const params = new HttpParams()
      .set('email', email)
      .set('qunatity', quantity.toString())
      .set('ItemName', item.itemName.toString())
      .set('Price', item.price.toString());

    return this._http.post('http://localhost:5214/api/Customer/AddToCart', null, {
      params,
      responseType: 'text',
      headers: this.getHeaders()
    });
  }

  getCartItems() {
    const email = sessionStorage.getItem('userName') || '';
    const params = new HttpParams()
      .set('email', email);

    return this._http.get<ICart[]>('http://localhost:5214/api/Customer/GetCartItems', { params, headers: this.getHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  AddToOrder(orderItems: IOrderItem[]): Observable<IOrderItem[]> {
    return this._http.post<IOrderItem[]>('http://localhost:5214/api/Customer/AddCartItemsToOrder', orderItems, { headers: this.getHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  removeFromCart(id: number) {
    return this._http.delete(`http://localhost:5214/api/Customer/RemoveCartItem/${id}`, {
      responseType: 'text' as 'json',
      headers: this.getHeaders()
    }).pipe(
      catchError(this.errorHandler)
    );
  }

  submitUserInfo(userInfo: IUser) {
    return this._http.put<IUser>('http://localhost:5214/api/Customer/UpdateUserProfile', userInfo, { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
  }

  displayRatings(menuItemId: number) {
    const params = new HttpParams().set('menuItemId', menuItemId);
    return this._http
      .get<number>("http://localhost:5214/api/Customer/DisplayRatingsByItem", { params, headers: this.getHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  SetRating(rating: IRating) {
    return this._http.post<IRating>('http://localhost:5214/api/Customer/AddRating', rating, {
      responseType: 'text' as 'json',
      headers: this.getHeaders()
    })
  }

  getRatedItemsForUser(email: string) {
    const params = new HttpParams().set('email', email);
    return this._http.get<IRating[]>("http://localhost:5214/api/Customer/GetRatingDetails", { params, headers: this.getHeaders() });
  }

  AddBooking(booking: IBooking): Observable<IBooking> {
    return this._http.post<IBooking>('http://localhost:5214/api/Customer/AddBooking/add-booking', booking, {headers: this.getHeaders()})
  }

  errorHandler(error: HttpErrorResponse) {
    console.log(error);
    return throwError(error.message || "Server Error");
  }

}
