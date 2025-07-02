import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUser } from '../models/user';
import { ILogin } from '../models/login';
import { catchError, throwError } from 'rxjs';
import { ILoginResponse } from '../models/Response';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private _http: HttpClient) { }

  private getToken(): string | null {
    console.log(sessionStorage.getItem('token'));
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    console.log(new HttpHeaders({ 'Authorization': `Bearer ${token}` }));
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  loginUser(email: string, password: string) {
    const userObj: ILogin = { email, password };
    console.log("Login user");
    return this._http.post<ILoginResponse>(
      'http://localhost:5214/api/Auth/Login',
      userObj
    ).pipe(catchError(this.errorHandler));
  }

  validateCredentials(email: string, password: string) {
    const userObj: IUser = {
      email,
      fullName: '',
      password,
      gender: '',
      phoneNumber: '',
      address: '',
      roleId: 0
    };

    return this._http.post<IUser>(
      'http://localhost:5214/api/Auth/ValidateUserCredentials',
      userObj
    ).pipe(catchError(this.errorHandler));
  }

  registerUser(email: string, fullName: string, gender: string, password: string, phoneNumber: string, address: string) {
    const userObj: IUser = {
      email,
      fullName,
      gender,
      password,
      phoneNumber,
      address,
      roleId: 2
    };

    return this._http.post<IUser>(
      'http://localhost:5214/api/Auth/Register',
      userObj
    ).pipe(catchError(this.errorHandler));
  }

  getProfileStatus(email: string) {
    return this._http.get<IUser>(
      `http://localhost:5214/api/Auth/GetUserByMail/GetProfile?mail=${email}`,
      { headers: this.getHeaders() }
    ).pipe(catchError(this.errorHandler));
  }

  TestMail(email: string, status: string) {
    return this._http.get(
      `http://localhost:5214/api/Auth/TestEmail?email=${email}&status=${status}`,
      { responseType: 'text' as 'json' }
    ).pipe(catchError(this.errorHandler));
  }

  TestMail2(email: string, code: string) {
    return this._http.get(
      `http://localhost:5214/api/Auth/TestEmail2?email=${email}&code=${code}`,
      { responseType: 'text' as 'json' }
    ).pipe(catchError(this.errorHandler));
  }

  UpdateUserProfile(user: IUser) {
    return this._http.put<IUser>(
      'http://localhost:5214/api/Customer/UpdateUserProfile',
      user,
      { headers: this.getHeaders() }
    ).pipe(catchError(this.errorHandler));
  }

  errorHandler(error: HttpErrorResponse) {
    console.error(error);
    return throwError(error.message || "Server Error");
  }
}
