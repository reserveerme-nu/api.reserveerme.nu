using System.Collections.Generic;
using Model.Models;

namespace api.reserveerme.nu.ViewModels
{
    public class RoomViewModel
    {
        public string name { get; set; }
        public List<ReservationViewModel> Reservations { get; set; }

        public RoomViewModel()
        {
            
        }

        public RoomViewModel(Room room)
        {
            Reservations = new List<ReservationViewModel>();
            foreach (var r in room.Reservations)
            {
                Reservations.Add(new ReservationViewModel(r));
            }

            this.name = room.Name;
        }
    }
}