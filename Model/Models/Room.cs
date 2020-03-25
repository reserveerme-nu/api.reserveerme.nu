using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace Model.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Room()
        {
        }

        public Room(RoomViewModel room)
        {
            this.Name = room.name;
        }
    }
}