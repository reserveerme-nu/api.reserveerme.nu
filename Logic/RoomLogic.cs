using System;
using System.Collections.Generic;
using System.Linq;
using api.reserveerme.nu.ViewModels;
using Model.Models;
using Model.ViewModels;

namespace Logic
{
    public class RoomLogic : IRoomLogic
    {
        private List<RoomViewModel> _rooms = new List<RoomViewModel>();
        
        public int AddRoom(RoomViewModel roomViewModel)
        {
            _rooms.Add(roomViewModel);
            return _rooms.IndexOf(roomViewModel);
        }

        public RoomViewModel GetRoomById(int roomId)
        {
            return _rooms.ElementAt(roomId);
        }

        public IEnumerable<RoomViewModel> GetAllRooms()
        {
            return _rooms;
        }

        public bool RemoveRoom(int roomId)
        {
            _rooms.RemoveAt(roomId);
            return true;
        }
    }
}