import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { IUser } from '../../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangepasswordComponent {
  newPassword: string = '';
  confirmPassword: string = '';
  passwordMismatch: boolean = false;
  successMessage: string = '';
  email?: string;
  constructor(private _userservice: UserService, private _router:Router) {

  }

  submitReset(form: any) {
    if (this.newPassword !== this.confirmPassword) {
      this.passwordMismatch = true;
      this.successMessage = '';
      return;
    }

    this.passwordMismatch = false;
    const user: IUser = {
      email: sessionStorage.getItem('email') || '',
      password: this.newPassword,
      roleId: 0,        // or a valid default
      gender: '',       // or 'NotSpecified'
      address: '',
      fullName:' ',
      phoneNumber:' '
    };

    this._userservice.UpdateUserProfile(user).subscribe(
      (res) => {
        console.log('New password:', this.newPassword);
        this.successMessage = 'Password has been reset successfully.';
        form.resetForm();
        this._router.navigate(['/login']);
      },
      (err) => {
        console.error('Error updating password:', err);
      }
    );
  }
}
