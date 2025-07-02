import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { IIssue } from '../models/issue';

@Injectable({
  providedIn: 'root'
})
export class SupportService {
  issues: IIssue[] = [];

  constructor(private _http: HttpClient) { }

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  getAllissues() {
    let response = this._http.get<IIssue[]>('http://localhost:5214/api/Support/GetIssues', { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    return response;
  }

  UpdateIssueStatus(issueId: number, status: string): Observable<boolean> {
    var issue: IIssue = {
      issueId: issueId,
      orderItemId: 0,
      issueDescription: '',
      email: '',
      issueStatus: status
    };
    let response = this._http.put<boolean>("http://localhost:5214/api/Support/UpdateIssueStatus", issue, { headers: this.getHeaders() }).pipe(catchError(this.errorHandler));
    return response;
  }

  TestMail(email: string, status: string) {
    return this._http.get(
      `http://localhost:5214/api/Auth/TestEmail?email=${email}&status=${status}`,
      {
        responseType: 'text' as 'json'
      }

    ).pipe(
      catchError(this.errorHandler)
    );
  }

  errorHandler(error: HttpErrorResponse) {
    console.log(error);
    return throwError(error.message || "Server Error");
  }
}
