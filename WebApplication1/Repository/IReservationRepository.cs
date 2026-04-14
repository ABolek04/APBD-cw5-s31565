using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IReservationRepository
{
    IEnumerable<Reservation> GetAllReservations();
}