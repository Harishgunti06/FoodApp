import { Component, OnInit } from '@angular/core';
import { IUser } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.css']
})
export class PersonalInfoComponent implements OnInit{
  user: IUser | null = null;
  showMsg: boolean = false;

  constructor(private _userService: UserService) { }

  ngOnInit(): void {
    const email = sessionStorage.getItem('userName');
    if (email) {
      this._userService.getProfileStatus(email).subscribe(
        (resSuccess) => {
          this.user = resSuccess;
          this.showMsg = true;
        },
        (resError) => {
          this.user = null;
          this.showMsg = true;
          console.log(resError);
        },
        () => { console.log("Profile status can be shown now."); }
      );
    } else {
      this.showMsg = true;
      console.warn("No email found in sessionStorage");
    }
  }
}
