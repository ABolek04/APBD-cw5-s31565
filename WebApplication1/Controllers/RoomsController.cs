using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public static List<Room> Rooms = new List<Room>
        {
             new Room { Id = 1, Name = "Room1", BuildingCode = "A",Floor=1, Capacity = 10, HasProjector = true, IsActive = true },
             new Room { Id = 2, Name = "Room2", BuildingCode = "B",Floor=2, Capacity = 20, HasProjector = true, IsActive = true },
             new Room { Id = 3, Name = "Room3", BuildingCode = "C",Floor=6, Capacity = 30, HasProjector = false, IsActive = true },
             new Room { Id = 4, Name = "Room4", BuildingCode = "D",Floor=1, Capacity = 25, HasProjector = true, IsActive = false },
             new Room {Id = 5,Name = "Room5",BuildingCode = "A",Floor=4,Capacity = 5,HasProjector = false, IsActive = true}
        };

        // [HttpGet]
        // public ActionResult<IEnumerable<Room>> GetAllRooms()
        // {
        //     return Ok(Rooms);
        // } na dole metoda zwraca to samo bez filtrowania

        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetRooms([FromQuery] int? minCapacity,[FromQuery]bool? hasProjector,[FromQuery]bool? activeOnly)
        {
            var query = Rooms.AsEnumerable();
            if (hasProjector.HasValue)
            {
                query = query.Where(r => r.HasProjector == hasProjector.Value);
            }

            if (activeOnly.HasValue)
            {
                query = query.Where(r => r.IsActive == activeOnly.Value);
            }

            if (minCapacity.HasValue)
            {
                query = query.Where(r=>r.Capacity>=minCapacity.Value);
            }
            return Ok(query.ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Room> GetById([FromRoute]int id)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpGet("building/{BuildingCode}")]
        public ActionResult<Room> GetByBuildingCode([FromRoute] string buildingCode)
        {
            var room = Rooms.Where(bc => bc.BuildingCode == buildingCode).ToList();
            if (!room.Any())
            {
                return NotFound();
            }

            return Ok(room);
        }
        
        [HttpPost]
        public IActionResult CreateRoom([FromBody] Room newRoom)
        {
            newRoom.Id = Rooms.Any() ? Rooms.Max(id => id.Id) + 1 : 1;
            Rooms.Add(newRoom);
            return CreatedAtAction(nameof(GetById), new { id = newRoom.Id }, newRoom);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Room updateRoom)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            room.Name = updateRoom.Name;
            room.BuildingCode = updateRoom.BuildingCode;
            room.Floor = updateRoom.Floor;
            room.Capacity = updateRoom.Capacity;
            room.HasProjector = updateRoom.HasProjector;
            room.IsActive = updateRoom.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            if (ReservationsController.reservations.Any(r => r.RoomId == id))
            {
                return Conflict();
            }
            Rooms.Remove(room);
            return NoContent();
            
        }
    }
}
