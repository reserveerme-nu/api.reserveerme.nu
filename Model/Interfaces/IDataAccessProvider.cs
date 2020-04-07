using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Models;

namespace Model.Interfaces
{
    public interface IDataAccessProvider
    {
        Task Create(Room room);
        Task Add(Reservation reservation, int roomId);
        Task<Reservation> Read(int roomId, int reservationId);
        Task<string> GetStatus(int roomId);
        Task<List<Room>> ReadAll(int roomId);
        Task Update(Reservation reservation);
        Task Delete(Reservation reservation);
    }
}