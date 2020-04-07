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

        public async Task<string> GetStatus(int roomId)
        {
            var test = await _context.Rooms.FirstAsync(p => p.Id == roomId);
            var rooms = _context.Rooms
                .Include(b => b.Reservations)
                .ToList();
            var room = rooms.FirstOrDefault(p => p.Id == roomId);
            if (room != null)
            {
                var reservations = room.Reservations;
                foreach (var reservation in reservations)
                {
                    if (DateTime.Compare(DateTime.Now, reservation.DateStart) > 0 && DateTime.Compare(DateTime.Now, reservation.DateEnd) < 0)
                    {
                        return "occupied";
                    }
                }
                return "free";
            }

            throw new ArgumentException();
        }

        public async Task<bool> RemoveCurrentReservation(int roomId)
        {
            var test = await _context.Rooms.FirstAsync(p => p.Id == roomId);
            var rooms = _context.Rooms
                .Include(b => b.Reservations)
                .ToList();
            var room = rooms.FirstOrDefault(p => p.Id == roomId);
            Reservation r = null;
            if (room != null)
            {
                var reservations = room.Reservations;
                foreach (var reservation in reservations)
                {
                    if (DateTime.Compare(DateTime.Now, reservation.DateStart) > 0 && DateTime.Compare(DateTime.Now, reservation.DateEnd) < 0)
                    {
                        r = reservation;
                        break;
                    }
                }
            }

            if (r != null)
            {
                room.Reservations.Remove(r);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Room>> ReadAll(int roomId)
        {
            var room = await _context.Rooms.FirstAsync(p => p.Id == roomId);
            var reservations = _context.Rooms
                .Include(b => b.Reservations)
                .ToList();
            return reservations;
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