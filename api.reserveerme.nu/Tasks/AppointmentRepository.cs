using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace api.reserveerme.nu.Tasks
{
    public static class AppointmentRepository
    {
        public static List<AppointmentViewModel> Appointments;

        public static List<AppointmentViewModel> getAppointments()
        {
            if (Appointments == null)
            {
                Appointments = new List<AppointmentViewModel>();
            }
            
            return Appointments;
        }

        public static void setAppointments(List<AppointmentViewModel> appointments)
        {
            Appointments = appointments;
        }
    }
}