using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IRoomRepository
{
    IEnumerable<Room> GetAllRooms();
    Room GetRoomById(int id);
    
}