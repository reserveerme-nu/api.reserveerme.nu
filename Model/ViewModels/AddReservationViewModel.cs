namespace api.reserveerme.nu.ViewModels
{
    public class AddReservationViewModel
    {
        public int RoomId { get; set; }
        public string DateStart { get; set; }
        public int Duration { get; set; }
        public string Issuer { get; set; }
    }
}
