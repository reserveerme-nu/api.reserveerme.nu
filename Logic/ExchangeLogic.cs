using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using Model.Enums;
using Model.Exceptions;
using Model.Models;

namespace Logic
{
    public class ExchangeLogic : IExchangeLogic
    {
        
        ExchangeDataContext exchange = new ExchangeDataContext();
        
        public IEnumerable<AppointmentViewModel> GetAppointments()
        {
            var appointments = new List<AppointmentViewModel>();

            if (exchange.GetAppointments().Count() != 0)
            {
                foreach (var appointment in exchange.GetAppointments())
                {
                    string host;
                    if (appointment.RequiredAttendees.Count > 0)
                    {
                        host = appointment.RequiredAttendees.First().Name;
                    }
                    else
                    {
                        host = "Unknown Host";
                    }
                    var avm = new AppointmentViewModel
                    {
                        Id = appointment.Id.UniqueId,
                        Subject = appointment.Subject,
                        Body = host,
                        Start = appointment.Start,
                        End = appointment.End,
                        Location = appointment.Location,
                        Status = appointment.Categories.FirstOrDefault()
                        
                    };

                    appointments.Add(avm);
                }

                return appointments;
            }
            throw new CalenderEmptyException();
        }
        
        public void CreateNewAppointment(AppointmentViewModel avm)
        {
            foreach (var appointment in exchange.GetAppointments())
            {
                if (avm.Start <= appointment.End && appointment.Start <= avm.End)
                {
                    throw new AppointmentTimeSlotNotAvailableException();
                }
            }
            exchange.CreateNewAppointment(avm);
        }

        public void SetCredentials(string username, string password)
        {
            exchange.SetCredentials(username, password);
        }

        public void StartMeeting(int roomId)
        {
            exchange.SetRoomStatus(roomId, StatusType.Occupied);
        }
    }
}