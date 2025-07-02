import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICategories } from '../models/Category';
import { catchError, throwError } from 'rxjs';
import { IMenuItem } from '../models/menuItem';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  foodCategories: ICategories[] = [];

  constructor(private _http: HttpClient) { }

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }
  getCategories() {

    var temp = this._http.get<ICategories[]>("http://localhost:5214/api/Customer/GetAllCategories", { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    console.log(temp);
    return temp;
  }

  getProductsByCategoryId(categoryId: number) {
    const params = new HttpParams().set('categoryId', categoryId.toString());
    const url = "http://localhost:5214/api/Customer/GetMenuItemsByCategory";

    return this._http.get<IMenuItem[]>(url, { params, headers: this.getHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  errorHandler(error: HttpErrorResponse) {
    console.error(error);
    return throwError(error.message || "Server Error");
  }

}
