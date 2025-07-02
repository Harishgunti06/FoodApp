import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent {

  generatedCode: string = '';
  email: string = '';
  enteredCode: string = '';
  codeSent: boolean = false;
  statusMessage: string = '';
  constructor(private _userService: UserService, private _router: Router) {

  }

  sendCode() {
    this.generatedCode = Math.floor(100000 + Math.random() * 900000).toString();

    this._userService.TestMail2(this.email, this.generatedCode).subscribe(
      (resSuccess) => { this.codeSent=true },
      (resError) => { console.log(resError); },
      () => { console.log("Sent Successfully"); }

    )
  }

  verifyCode() {
    if (this.enteredCode === this.generatedCode) {
      this.statusMessage = 'Code verified! You can now reset your password.';
      sessionStorage.setItem("email",this.email);
      this._router.navigate(['/changepassword'])
      // Proceed to password reset page or show reset form
    } else {
      this.statusMessage = 'Invalid code. Please try again.';
    }
  }

}
