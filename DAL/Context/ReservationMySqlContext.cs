using Model.Models;

namespace DAL.MySQL.Contexts
{
    public class ReservationMySqlContext : MySqlContext<Reservation>
    {
        public ReservationMySqlContext(string connectionString) : base(connectionString)
        {
        }
    }
}