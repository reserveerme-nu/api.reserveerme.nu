using System;
using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace Logic
{
    public interface IExchangeLogic
    {
        public IEnumerable<AppointmentViewModel> GetAppointments();
        public void CreateNewAppointment(AppointmentViewModel avm);
        public bool EndCurrentAppointment(int roomId);
        public bool EndAppointment(AppointmentViewModel a);
        public void SetCredentials(string username, string password);
        void StartMeeting(int viewModelRoomId);
        AppointmentViewModel GetCurrentAppointment(int roomId);
    }
}