using System;
using Model.Models;

namespace api.reserveerme.nu.ViewModels
{
    public class ReservationViewModel
    {
        public int RoomId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public String Issuer { get; set; }

        public ReservationViewModel()
        {
            
        }
        
        public ReservationViewModel(Reservation reservation)
        {
            this.DateStart = reservation.DateStart;
            this.DateEnd = reservation.DateEnd;
            this.Issuer = reservation.Issuer;
        }
    }
}