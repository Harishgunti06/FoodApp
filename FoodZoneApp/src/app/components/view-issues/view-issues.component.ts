import { Component } from '@angular/core';
import { IIssue } from '../../models/issue';
import { SupportService } from '../../services/support.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-issues',
  templateUrl: './view-issues.component.html',
  styleUrls: ['./view-issues.component.css']
})
export class ViewIssuesComponent {
  issues: IIssue[] = [];
  filteredIssues: IIssue[] = [];
  showMessage: boolean = false;

  constructor(private _supportService: SupportService, private _router: Router) { }

  ngOnInit() {
    this._supportService.getAllissues().subscribe(
      (resSuccess) => {
        this.issues = resSuccess;
        this.filteredIssues = this.issues;
        if (this.issues.length == 0) {
          this.showMessage = true;
        }
      },
      (resError) => {
        this.showMessage = true;
        this.issues = [];
        this.filteredIssues = [];
        console.log(resError);
      },
      () => { console.log("Issues fetched successfully."); }
    )
  }

  UpdateIssue(issue:IIssue) {
    this._router.navigate(['/update-issue', issue.issueId, issue.issueDescription,issue.orderItemId,issue.email,issue.issueStatus]);
  }

  goBack() {
    this._router.navigate(['/support']);
  }

  logout() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    this._router.navigate(['']);
  }
}
