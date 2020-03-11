using System;

namespace Model.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public User Issuer { get; set; }
    }
}