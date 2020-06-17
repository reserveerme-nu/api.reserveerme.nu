using System.Collections.Generic;
using api.reserveerme.nu.ViewModels;

namespace api.reserveerme.nu.Tasks
{
    public static class AppointmentRepository
    {
        private static IEnumerable<AppointmentViewModel> _appointments;

        public static IEnumerable<AppointmentViewModel> GetAppointments()
        {
            return _appointments;
        }

        public static void SetAppointments(IEnumerable<AppointmentViewModel> appointments)
        {
            _appointments = appointments;
        }
    }
}