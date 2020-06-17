using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using api.reserveerme.nu.Tasks.Scheduling;
using api.reserveerme.nu.WSControllers;
using DAL;
using Logic;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.reserveerme.nu.Tasks
{
    public class CheckExpiredReservationsTask : IScheduledTask
    {
        public string Schedule => "*/1 * * * *";
        public IExchangeLogic ExchangeLogic;

        public CheckExpiredReservationsTask(IExchangeLogic exchangeLogic)
        {
            ExchangeLogic = exchangeLogic;
        }
        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var appointments = ExchangeLogic.GetAppointments();
            foreach (var appointment in appointments)
            {
                if (DateTime.Compare(DateTime.Now, appointment.Start) > 0 && DateTime.Compare(DateTime.Now, appointment.End) < 0)
                {
                    var date = appointment.Start;
                    if (DateTime.Compare(DateTime.Now, date.AddMinutes(1)) > 0 && appointment.Status != "Occupied");
                    {
                        ExchangeLogic.EndAppointment(appointment);
                        WebsocketRepository.GetWebSocketServer().WebSocketServices.BroadcastAsync("update", () => {});
                    }
                }
            }

            if (AppointmentRepository.GetAppointments() == null)
            {
                AppointmentRepository.SetAppointments(appointments);
            }

            if (!AppointmentRepository.GetAppointments().Equals(appointments))
            {
                WebsocketRepository.GetWebSocketServer().WebSocketServices.BroadcastAsync("update", () => {});
            }
        }
    }
}