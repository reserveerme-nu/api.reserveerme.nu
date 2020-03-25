using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Models;

namespace Model.Interfaces
{
    public interface IDataAccessProvider
    {
        Task Create(Room room);
        Task Add(Reservation reservation);
        Task<Reservation> Read(int reservationId);
        Task<List<Reservation>> ReadAll(int reservationId);
        Task Update(Reservation reservation);
        Task Delete(Reservation reservation);
    }
}