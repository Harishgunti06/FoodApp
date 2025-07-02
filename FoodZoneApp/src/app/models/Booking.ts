export interface IBooking {
  bookingId: number,
  email: string,
  bookingDate: string,
  bookingTime: string;
  guests: number,
  checkedIn: boolean,
}
