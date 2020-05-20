using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using DAL.MySQL.Contexts;
using Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Interfaces;

namespace api.reserveerme.nu
{
    public class CheckService : IHostedService
    {
        private readonly MySqlContext _context;
        
        public CheckService()
        {
            var contextFactory = new DesignTimeDbContextFactory();
            _context = contextFactory.CreateDbContext(new string[0]);
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var rooms = _context.Rooms.ToList();
            foreach (var room in rooms)
            {
                foreach (var reservation in room.Reservations)
                {
                    if (DateTime.Compare(DateTime.Now, reservation.DateStart) > 0 && DateTime.Compare(DateTime.Now, reservation.DateEnd) < 0)
                    {
                        var date = reservation.MeetingDateStart;
                        if (reservation.MeetingDateStart == DateTime.MinValue && date.AddMinutes(1) > DateTime.Now)
                        {
                            reservation.DateEnd = DateTime.Now;
                        }
                    }
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}