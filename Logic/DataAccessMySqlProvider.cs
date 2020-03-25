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

        public async Task Add(Reservation reservation)
        {
            var room = await _context.Rooms.FirstAsync(p => p.Name == reservation.Room.Name);
            reservation.Room = room;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Reservation> Read(int reservationId)
        {
            var reservation = await _context.Reservations.FirstAsync(p => p.Id == reservationId);
            return reservation;
        }

        public Task<List<Reservation>> ReadAll(int roomId)
        {
            var room = _context.Rooms.FirstAsync(p => p.Id == roomId);
            var reservations = _context.Entry(room).Collection("Rooms").Load();
            return null;
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