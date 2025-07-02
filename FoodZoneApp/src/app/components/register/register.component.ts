import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  registrationMessage: string = '';
  errorMessage: string = '';

  constructor(private _userService: UserService, private _router: Router) { }

  register(form: NgForm) {
    if (form.invalid) {
      this.errorMessage = 'Please fill all required fields';
      return;
    }

    this._userService.registerUser(
      form.value.email,
      form.value.fullName,
      form.value.gender,
      form.value.password,
      form.value.phoneNumber,
      form.value.address
    ).subscribe(
      (resSuccess) => {
        this.registrationMessage = `Registered: ${resSuccess.fullName}`;
        this.errorMessage = '';
        this._router.navigate(['/login']);
      },
      (resError : HttpErrorResponse) => {
        this.errorMessage = resError.error|| 'Registration failed.';
        this.registrationMessage = '';
      },
      () => {
        console.log('Registration request completed.');
      }
    );
  }
}
