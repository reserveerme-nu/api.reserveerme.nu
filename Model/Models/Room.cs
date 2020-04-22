using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace Model.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public Room()
        {
        }

        public Room(RoomViewModel room)
        {
            this.Reservations = new List<Reservation>();
            this.Name = room.name;
            if (room.Reservations != null)
            {
                foreach (var r in room.Reservations)
                {
                    this.Reservations.Add(new Reservation(r));
                }
            }
        }
    }
}