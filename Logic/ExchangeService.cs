using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using api.reserveerme.nu.ViewModels;
using DAL;
using Microsoft.Exchange.WebServices.Data;
using Model.Models;

namespace Logic
{
    public class ExchangeService
    {
        
        ExchangeDataContext exchange = new ExchangeDataContext();
        
        public IEnumerable<AppointmentViewModel> GetAppointments()
        {
            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();

            foreach (var appointment in exchange.GetAppointments())
            {
                AppointmentViewModel avm = new AppointmentViewModel();
                avm.Subject = appointment.Subject;
                avm.Body = appointment.Body.Text;
                
                appointments.Add(avm);
            }

            return appointments;
        }
    }
}