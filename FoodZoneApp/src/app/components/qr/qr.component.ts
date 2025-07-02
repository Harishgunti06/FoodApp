import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QRCodeModule } from 'angularx-qrcode';
@Component({
  selector: 'app-qr',
  templateUrl: './qr.component.html',
  styleUrls: ['./qr.component.css']
})
export class QrComponent {
  bookingId = '';
  bookingdate = '';
  bookingtime = '';
  guests = 0;
  qrData = '';

  constructor(private _router: ActivatedRoute) { }

  ngOnInit() {
    this._router.queryParams.subscribe(params => {
      this.bookingId = params['bookingId'];
      this.bookingdate = params['bookingDate'];
      this.bookingtime = params['bookingTime'];
      this.guests = params['guests'];
      this.qrData = `http://localhost:4200/login`;
    });
  }

  }
