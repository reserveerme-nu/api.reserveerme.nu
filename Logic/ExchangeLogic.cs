using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using Model.Exceptions;
using Model.Models;

namespace Logic
{
    public class ExchangeLogic
    {
        
        ExchangeDataContext exchange = new ExchangeDataContext();
        
        public IEnumerable<AppointmentViewModel> GetAppointments()
        {
            var appointments = new List<AppointmentViewModel>();

            foreach (var appointment in exchange.GetAppointments())
            {
                var avm = new AppointmentViewModel
                {
                    Subject = appointment.Subject,
                    Body = appointment.Body.Text,
                    Start = appointment.Start,
                    End = appointment.End,
                    Location = appointment.Location
                };

                appointments.Add(avm);
            }

            return appointments;
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
    }
}