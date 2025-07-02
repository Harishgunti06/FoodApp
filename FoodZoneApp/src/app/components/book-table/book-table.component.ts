import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { IBooking } from '../../models/Booking';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-book-table',
  templateUrl: './book-table.component.html',
  styleUrls: ['./book-table.component.css']
})
export class BookTableComponent {


  timeSlots: string[] = [
    '10:00:00', '11:30:00', '13:00:00',
    '15:00:00', '18:00:00', '20:00:00'
  ];

  booking: IBooking = {
    bookingId: 0,
    email: sessionStorage.getItem("userName") || '',
    bookingDate: '',
    bookingTime: '',
    guests: 0,
    checkedIn: false
  };

  constructor(private _customerservice: CustomerService, private _router: Router) { }

  submitBooking(bookingForm: NgForm) {
    console.log(bookingForm.value);
    const today = new Date();
    const selectedDate = new Date(bookingForm.value.bookingDate);

    // Set both dates to midnight to only compare date parts (ignoring time)
    today.setHours(0, 0, 0, 0);
    selectedDate.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
      alert("Date cannot be less than today.");
      return;
    }
    this._customerservice.AddBooking(this.booking).subscribe(
      (resSuccess) => {
        alert('Your booking has been submitted!');
        console.log(resSuccess);
        console.log(resSuccess.bookingId);
        this._router.navigate(['/qr', resSuccess.bookingId]);
      },
      (resError) => { console.log(resError); },
      () => { console.log("Booking Added"); }
    );
  }
}
