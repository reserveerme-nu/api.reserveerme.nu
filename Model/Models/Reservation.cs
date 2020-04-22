using System;
using api.reserveerme.nu.ViewModels;

namespace Model.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public String Issuer { get; set; }
        public int RoomId { get; set; }

        public Reservation()
        {
            
        }

        public Reservation(ReservationViewModel reservationViewModel)
        {
            this.DateStart = reservationViewModel.DateStart;
            this.DateEnd = reservationViewModel.DateEnd;
            this.Issuer = reservationViewModel.Issuer;
            this.RoomId = reservationViewModel.RoomId;
        }

        public Reservation(InstantReservationViewModel reservationViewModel)
        {
            //this.DateStart = RoundUp(DateTime.Now, TimeSpan.FromMinutes(15));
            this.DateStart = DateTime.Now;
            this.DateEnd = this.DateStart.AddMinutes(reservationViewModel.Duration);
            this.RoomId = reservationViewModel.RoomId;
            this.Issuer = reservationViewModel.Issuer;
        }
        
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}