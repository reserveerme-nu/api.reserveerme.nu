using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;

namespace DAL
{
    public class ExchangeDataContext
    {
        private ExchangeService Service
        {
            get
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010);
                service.Credentials = new WebCredentials("409336@student.fontys.nl", "wachtwoord");
                service.Url = new Uri("https://outlook.office365.com/ews/exchange.asmx");
                return service;
            }
        }
        
        public IEnumerable<Appointment> GetAppointments()
        {
            // this week
            DateTime startDate = DayPilot.Utils.Week.FirstDayOfWeek();
            DateTime endDate = startDate.AddDays(7);

            // load the default calendar
            CalendarFolder calendar = CalendarFolder.Bind(Service, WellKnownFolderName.Calendar, new PropertySet());

            // load events
            CalendarView cView = new CalendarView(startDate, endDate, 50);
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.Id);
            FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);

            Service.LoadPropertiesForItems(appointments, PropertySet.FirstClassProperties);
            
            return appointments;
        }
    }
}