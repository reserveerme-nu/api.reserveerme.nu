using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using api.reserveerme.nu.Tasks.Scheduling;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.reserveerme.nu.Tasks
{
    public class CheckExpiredReservationsTask : IScheduledTask
    {
        public string Schedule => "*/1 * * * *";
        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var contextFactory = new DesignTimeDbContextFactory();
            var context = contextFactory.CreateDbContext(new string[0]);
            var rooms = await context.Rooms
                .Include(b => b.Reservations)
                .ToListAsync(cancellationToken);
            foreach (var room in rooms)
            {
                foreach (var reservation in room.Reservations)
                {
                    if (DateTime.Compare(DateTime.Now, reservation.DateStart) > 0 && DateTime.Compare(DateTime.Now, reservation.DateEnd) < 0)
                    {
                        var date = reservation.DateStart;
                        if (reservation.MeetingDateStart == DateTime.MinValue && DateTime.Compare(DateTime.Now, date.AddMinutes(1)) > 0)
                        {
                            reservation.DateEnd = DateTime.Now;
                            await context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
            }
        }
    }
}