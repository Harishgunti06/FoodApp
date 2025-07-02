import { Component } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent {
  formData = {
    email: '',
    fullName: '',
    phoneNumber: '',
    password: '',
    address:'',
  };

  constructor(private _customerservice: CustomerService, private _router: Router) {
    const email = sessionStorage.getItem('userName') || '';
    this.formData.email = email;

    const rawName = email.split('@')[0].replaceAll('.', ' ');

    this.formData.fullName = this.capitalize(rawName);
  }

  capitalize(name: string): string {
    return name.charAt(0).toUpperCase() + name.slice(1);
  }

  submitForm(form: NgForm) {
    console.log(form.value);
    this._customerservice.submitUserInfo(form.value).subscribe(
      (resSuccess) => {

        alert("Profile Update Successfully")
      },
      (resError) => {
        console.error('Error:', resError);
      },
      () => { console.log("Update Success"); }
    )
  }
  goBack() {
    this._router.navigate(["/customer"]);
  }
}
