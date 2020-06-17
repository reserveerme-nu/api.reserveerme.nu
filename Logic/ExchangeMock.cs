using api.reserveerme.nu.ViewModels;
using Model.Enums;
using Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class ExchangeMock : IExchangeLogic
    {
        private readonly List<AppointmentViewModel> appointments;

        public ExchangeMock(List<AppointmentViewModel> appointments)
        {
            this.appointments = appointments;
        }

        public void CreateNewAppointment(AppointmentViewModel avm)
        {
            foreach (var appointment in appointments)
            {
                if (avm.Start <= appointment.End && appointment.Start <= avm.End)
                {
                    throw new AppointmentTimeSlotNotAvailableException();
                }
            }
            appointments.Add(avm);
        }

        public bool EndAppointment(AppointmentViewModel a)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == a.Id.ToString())
                {
                    appointments.Remove(appointment);
                    return true;
                }
            }
            throw new AppointmentNotExistantException("Appointment with ID: " + a.Id + " was not found");
        }

        public bool EndCurrentAppointment(int roomId)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == roomId.ToString())
                {
                    if (appointment.Start <= DateTime.Now && appointment.End <= DateTime.Now)
                    {
                        appointments.Remove(appointment);
                        return true;
                    }
                    return false;
                }
            }
            throw new RoomNotFoundException("Room with ID: " + roomId + " was not found");
        }

        public IEnumerable<AppointmentViewModel> GetAppointments()
        {
            return appointments;
        }

        public AppointmentViewModel GetCurrentAppointment(int roomId)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == roomId.ToString())
                {
                    return appointment;
                }
            }
            throw new RoomNotFoundException("Room with ID: " + roomId + " was not found");
        }

        public void SetCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void StartMeeting(int viewModelRoomId)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == viewModelRoomId.ToString())
                {
                    appointment.Status = StatusType.Occupied.ToString();
                    return;
                }
            }
            throw new RoomNotFoundException("Room with ID: " + viewModelRoomId + " was not found");
        }
    }
}
