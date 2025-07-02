import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  message: string;
  showDiv: boolean;
 

  constructor(private _userService: UserService, private _router: Router) {
    this.message = "";
    this.showDiv = false;
   
  }

  login(form: NgForm) {
    this._userService.loginUser(form.value.email, form.value.password).subscribe(
      (responseLoginStatus) => {
        this.showDiv = true;
        if (responseLoginStatus && responseLoginStatus.token) {
          const role = responseLoginStatus.roleId.toString();
          console.log("userrole:" + role);
          const email = responseLoginStatus.email;
          console.log("email:" + email);
          const fullName = responseLoginStatus.fullName;
          console.log("fullName:" + fullName);

          this.message = "Valid credentials";
          if (role === "1") {
            this._router.navigate(['/admin']);
          }
          else if (role === "2") {
            this._router.navigate(['/customer']);
          }
          else {
            this._router.navigate(['/support']);
          }
          sessionStorage.setItem('token', responseLoginStatus.token);
          sessionStorage.setItem('userName', responseLoginStatus.email); // or response.userName
          sessionStorage.setItem('userRole', role);

        }
        else {
          this.message = "Invalid Credentials Please Try Again";
          
        }
      },
      (resError) => {
        console.log(resError);
        this.message = "Error occurred while logging in";
        this.showDiv = true;
      },
      () => { console.log("Validate credentials executed"); }
    );
  }
  ngOnInit() {
    this.message = "";
    this.showDiv = false;
  }
  
}
