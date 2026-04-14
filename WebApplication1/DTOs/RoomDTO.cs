namespace WebApplication1.DTOs;

public class RoomDTO
{
    public int Id {get; set;}
    public string Name {get; set;}=string.Empty;
    public string BuildingCode {get; set;}=string.Empty;
    public int Capacity {get; set;}
    public bool HasProjector {get; set;}
    public bool IsActive {get; set;}
}