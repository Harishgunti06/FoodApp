import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-raise-issue',
  templateUrl: './raise-issue.component.html',
  styleUrls: ['./raise-issue.component.css']
})
export class RaiseIssueComponent {
  constructor(private _customerservice: CustomerService, private _router:Router) {

  }
  raiseIssue(form: NgForm) {
    this._customerservice.RaiseIssue(form.value.Id, form.value.description).subscribe(
      (resSuccess) => {
        console.log(resSuccess);
        alert("Issue Raised Successfully");
        
      },
      (resError) => {
        console.log(resError);
      },
      () => {
        console.log("Request Completed");
      }
      )
  }
  goBack() {
    this._router.navigate(['/customer']);
  }   
  } 
