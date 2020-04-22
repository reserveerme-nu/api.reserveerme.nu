using Model.Enums;

namespace Model.Models
{
    public class Status
    {
        public Reservation Reservation { get; set; }
        public StatusType StatusType { get; set; }
    }
}