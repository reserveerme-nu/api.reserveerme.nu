using System;
using api.reserveerme.nu.ViewModels;

namespace Model.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public User Issuer { get; set; }
        public int RoomId { get; set; }

        public Reservation()
        {
            
        }

        public Reservation(ReservationViewModel reservationViewModel)
        {
            this.DateStart = reservationViewModel.DateStart;
            this.DateEnd = reservationViewModel.DateEnd;
            this.Issuer = new User(reservationViewModel.Issuer);
            this.RoomId = reservationViewModel.RoomId;
        }
    }
}