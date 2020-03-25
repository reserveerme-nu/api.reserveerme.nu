using System;
using Model.Models;

namespace api.reserveerme.nu.ViewModels
{
    public class ReservationViewModel
    {
        public RoomViewModel Room { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public UserViewModel Issuer { get; set; }

        public ReservationViewModel()
        {
            
        }
        
        public ReservationViewModel(Reservation reservation)
        {
            this.DateStart = reservation.DateStart;
            this.DateEnd = reservation.DateEnd;
            this.Issuer = new UserViewModel(reservation.Issuer);
        }
    }
}