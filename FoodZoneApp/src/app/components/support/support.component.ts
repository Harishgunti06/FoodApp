import { Component } from '@angular/core';
import { SupportService } from '../../services/support.service';
import { Router } from '@angular/router';
import { IIssue } from '../../models/issue';

@Component({
  selector: 'app-support',
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.css']
})
export class SupportComponent {
  issues: IIssue[] = [];
  filteredIssues: IIssue[] = [];
  showMessage: boolean = false;

  constructor(private _router: Router) { }

  logout() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    this._router.navigate(['']);
  }
}
