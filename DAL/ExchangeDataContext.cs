using System;
using System.Collections.Generic;
using System.Linq;
using api.reserveerme.nu.ViewModels;
using Microsoft.Exchange.WebServices.Data;
using Model.Exceptions;

namespace DAL
{
    public class ExchangeDataContext
    {
        private string _username;
        private string _password;
        
        private ExchangeService Service
        {
            get
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010);
                service.Credentials = new WebCredentials(_username, _password);
                service.Url = new Uri("https://outlook.office365.com/ews/exchange.asmx");
                return service;
            }
        }

        public void SetCredentials(string username, string password)
        {
            _username = username;
            _password = password;
        }
        
        public IEnumerable<Appointment> GetAppointments()
        {
            // this week
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);

            // load the default calendar
            CalendarFolder calendar = CalendarFolder.Bind(Service, WellKnownFolderName.Calendar, new PropertySet());

            // load events
            CalendarView cView = new CalendarView(startDate, endDate, 50);
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.Id);
            FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);
            if (appointments.Items.Count == 0)
            {
                throw new CalenderEmptyException();
            }

            Service.LoadPropertiesForItems(appointments, PropertySet.FirstClassProperties);
             
            return appointments;
        }
        
        public void CreateNewAppointment(AppointmentViewModel avm)
        {
            var appointment = new Appointment(Service)
            {
                Subject = avm.Subject,
                Body = avm.Body,
                Start = avm.Start,
                End = avm.End,
                Location = avm.Location
            };
            
            // Save the appointment to calendar.
            appointment.Save(SendInvitationsMode.SendToNone);
            
            // Verify that the appointment was created by using the appointment's item ID.
            var item = Item.Bind(Service, appointment.Id, new PropertySet(ItemSchema.Subject));
            
            Console.WriteLine("\nAppointment created: " + item.Subject + "\n");
        }
    }
}