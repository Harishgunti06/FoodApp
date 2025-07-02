import { Component } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { IBooking } from '../models/Booking';

@Component({
  selector: 'app-conformation',
  templateUrl: './conformation.component.html',
  styleUrls: ['./conformation.component.css']
})
export class ConformationComponent {
  booking: IBooking[] = []
  constructor(private _adminservice: AdminService) { }

  ngOnInit() {
    this._adminservice.ViewBookings().subscribe(
      (resSuccess) => {
        this.booking = resSuccess
      },
      (resError) => {
        console.log(resError);
      },
      () => {
        console.log("Rated Items Retrieved");
      }
    );
  }

  checkIn(bookingId: number): void {
    console.log(bookingId);
    this._adminservice.checkIn(bookingId).subscribe(
     
      (resSuccess) => {
        alert("Check in Successfull")
        this.ngOnInit()
      },
      (resError) => {
        alert("Check-in failed");
        console.log(resError);
      },
      () => { console.log("Check in Successfull"); }
    );
  }
}
