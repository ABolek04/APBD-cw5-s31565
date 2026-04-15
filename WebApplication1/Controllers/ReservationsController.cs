using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public static List<Reservation> reservations = new List<Reservation>
        {
            new Reservation
            {
                Id = 1,
                RoomId = 1,
                OrganizerName = "Kacper",
                Topic = "Zabawa",
                StartTime = new DateTime(2026, 1, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 1, 1, 13, 0, 0),
                status = "planned"
            },
            new Reservation
            {
                Id = 2,
                RoomId = 2,
                OrganizerName = "Anna",
                Topic = "Warsztaty",
                StartTime = new DateTime(2026, 5, 11, 9, 0, 0),
                EndTime = new DateTime(2026, 5, 11, 15, 0, 0),
                status = "planned"
            },
            new Reservation
            {
                Id = 3,
                RoomId = 3,
                OrganizerName = "Michal",
                Topic = "Lekcja",
                StartTime = new DateTime(2026, 5, 12, 14, 0, 0),
                EndTime = new DateTime(2026, 5, 12, 15, 30, 0),
                status = "confirmed"
            },
            new Reservation
            {
                Id = 4,
                RoomId = 1,
                OrganizerName = "Adam",
                Topic = "Szkolenie",
                StartTime = new DateTime(2026, 5, 13, 8, 0, 0),
                EndTime = new DateTime(2026, 5, 13, 10, 0, 0),
                status = "planned"
            },
            new Reservation
            {
                Id = 5,
                RoomId = 1,
                OrganizerName = "Adam",
                Topic = "Konsultacje",
                StartTime = new DateTime(2026, 5, 14, 16, 0, 0),
                EndTime = new DateTime(2026, 5, 14, 17, 0, 0),
                status = "planned"
            }
        };

        // [HttpGet]
        // public ActionResult<Reservation> GetAllReservations()
        // {
        //     return Ok(reservations);
        // } 

        [HttpGet("{id}")]
        public ActionResult<Reservation> GetById(int id)
        {
            var reservation = reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        //GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetByQuery([FromQuery] DateTime? date, [FromQuery] string? status,
            [FromQuery] int? roomId)
        {
            var query = reservations.AsEnumerable();
            if (date.HasValue)
            {
                query = query.Where(r => r.StartTime.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.status.Equals(status));
            }

            if (roomId.HasValue)
            {
                query = query.Where(r => r.RoomId == roomId.Value);
            }

            return query.ToList();
        }

        [HttpPost]
        public ActionResult<Reservation> CreateReservation(Reservation newReservation)
        {
            var room = RoomsController.Rooms.FirstOrDefault(r => r.Id == newReservation.RoomId);
            if (room == null || !room.IsActive)
            {
                return BadRequest("sala nie istnieje lub nie jest aktywna");
            }

            if (IsConflict(newReservation))
            {
                return Conflict("Ta sala ma juz rezerwacje w tym terminie");
            }

            newReservation.Id = reservations.Any() ? reservations.Max(r => r.Id) + 1 : 1;
            reservations.Add(newReservation);
            return CreatedAtAction(nameof(GetById), new { id = newReservation.Id }, newReservation);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Reservation> UpdateReservation([FromRoute] int? id,
            [FromBody] Reservation updateReservation)
        {
            var exists = reservations.FirstOrDefault(r => r.Id == id);
            if (exists == null)
            {
                return NotFound();
            }

            var room = RoomsController.Rooms.FirstOrDefault(r => r.Id == updateReservation.RoomId);
            if (room == null || !room.IsActive)
            {
                return BadRequest("Pokoj nie istnieje lub jest nieaktywny");
            }

            if (IsConflict(updateReservation, id))
            {
                return Conflict("W tym czasie jest inna rezerwacja");
            }
            exists.RoomId = updateReservation.RoomId;
            exists.OrganizerName = updateReservation.OrganizerName;
            exists.Topic = updateReservation.Topic;
            exists.StartTime = updateReservation.StartTime;
            exists.EndTime = updateReservation.EndTime;
            exists.status = updateReservation.status;
            return Ok(exists);
        } 
        [HttpDelete("{id:int}")]
        public ActionResult<Reservation> DeleteReservation([FromRoute] int id)
        {
            var reservation = reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservations.Remove(reservation);
            return NoContent();
        }
        public bool IsConflict(Reservation reservation,int? excludeId = null)
        {
            return reservations.Any(
                istniejaca=>
                    istniejaca.RoomId == reservation.RoomId && 
                    istniejaca.Id != excludeId &&
                    istniejaca.StartTime > reservation.EndTime && 
                    istniejaca.EndTime < reservation.StartTime);
        }
        
    }
}
