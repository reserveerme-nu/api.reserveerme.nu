using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;
using Model.ViewModels;

namespace Logic
{
    public interface IRoomLogic
    {
        public int AddRoom(RoomViewModel roomViewModel);
        public RoomViewModel GetRoomById(int roomId);
        public IEnumerable<RoomViewModel> GetAllRooms();
        public bool RemoveRoom(int roomId);
    }
}