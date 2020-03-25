using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using Model.Models;

namespace Logic
{
    public class DataAccessMySqlProvider : IDataAccessProvider
    {
        private readonly MySqlContext _context;

        public DataAccessMySqlProvider(MySqlContext context)
        {
            _context = context;
        }

        public async Task Create(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task Add(Reservation reservation, int roomId)
        {
            var room = await _context.Rooms.FirstAsync(p => p.Id == roomId);
            if (room.Reservations == null)
            {
                room.Reservations = new List<Reservation>();
            }
            room.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation> Read(int roomId, int reservationId)
        {
            var room = await _context.Rooms.FirstAsync(p => p.Id == roomId);
            return room.Reservations.First(p => p.Id == reservationId);
        }

        public Task Update(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}