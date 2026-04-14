using WebApplication1.Models;

namespace WebApplication1.Repository;

public class RoomRepository : IRoomRepository
{
    List<Room> rooms = new List<Room>();
    public IEnumerable<Room> GetAllRooms()
    {
        return rooms;
    }
}