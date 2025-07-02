import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-scanner',
  templateUrl: './scanner.component.html',
  styleUrls: ['./scanner.component.css']
})
export class ScannerComponent {
  successMessage = '';
  errorMessage = '';

  constructor(private http: HttpClient) { }

  //onCodeResult(result: string) {
  //  const url = new URL(result);
  //  const bookingId = url.searchParams.get('bookingId');
  //  if (!bookingId) {
  //    this.errorMessage = 'Invalid QR Code';
  //    return;
  //  }
    // DO API Method
    //  this.http.post(`/api/bookings/checkin/${bookingId}`, {})
    //    .subscribe({
    //      next: () => {
    //        this.successMessage = `Booking #${bookingId} checked in successfully!`;
    //        this.errorMessage = '';
    //      },
    //      error: () => {
    //        this.errorMessage = `Check-in failed for booking #${bookingId}.`;
    //        this.successMessage = '';
    //      }
    //    });
    //} catch {
    //  this.errorMessage = 'QR Code not recognized.';
    //}

  }
