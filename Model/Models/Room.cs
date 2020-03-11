using System.Collections.Generic;

namespace Model.Models
{
    public class Room
    {
        public int Id { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}