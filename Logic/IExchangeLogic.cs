using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace Logic
{
    public interface IExchangeLogic
    {
        public IEnumerable<AppointmentViewModel> GetAppointments();
        public void CreateNewAppointment(AppointmentViewModel avm);
        public void SetCredentials(string username, string password);
    }
}