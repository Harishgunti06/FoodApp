import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-delivery-details',
  templateUrl: './delivery-details.component.html',
  styleUrls: ['./delivery-details.component.css']
})
export class DeliveryDetailsComponent {
  subtotal: number = 0;
  deliveryFee: number = 30;
  total: number = 0;

  constructor(
    private router: Router,
    private sharedService: SharedService
  ) { }

  ngOnInit(): void {
    const state: any = history.state;
    if (state && state.subtotal !== undefined) {
      this.subtotal = state.subtotal;
      this.total = this.subtotal + this.deliveryFee;
    } else {
      console.warn("Subtotal not found in router state");
    }
  }

  // Called on form submit
  onSubmit(deliveryForm: NgForm): void {
    if (deliveryForm.valid) {
      console.log("Form Data:", deliveryForm.value);

      // Save the total to shared service
      this.sharedService.setTotalAmount(this.total);

      // Navigate to payment page
      this.router.navigate(['/paymentmethod']);
    } else {
      alert("Please fill all required fields.");
    }
  }
}
