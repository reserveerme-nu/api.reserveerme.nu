using System;
using System.Globalization;
using api.reserveerme.nu.ViewModels;
using Models;
using Models.Utils;

namespace Model.Models
{
    public class Reservation : DatabaseItem
    {
        [Property] public DateTime DateStart { get; set; }
        [Property] public DateTime DateEnd { get; set; }
        [Property] public DateTime MeetingDateStart { get; set; }
        [Property] public DateTime MeetingDateEnd { get; set; }
        [Property] public String Issuer { get; set; }
        [Property] public int RoomId { get; set; }

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

        public Reservation(AddReservationViewModel addReservationViewModel)
        {
            this.DateStart = Convert.ToDateTime(addReservationViewModel.DateStart);
            this.DateEnd = this.DateStart.AddMinutes(addReservationViewModel.Duration);
            this.Issuer = addReservationViewModel.Issuer;
            this.RoomId = addReservationViewModel.RoomId;
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