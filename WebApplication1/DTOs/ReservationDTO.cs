namespace WebApplication1.DTOs;

public class ReservationDTO
{
    public int Id {get; set;}
    public int RoomId {get; set;}
    public string OrganizerName {get; set;}
    public string Topic { get; set; }=string.Empty;
    public string StartTime {get; set;}
    public string EndTime {get; set;}
    public string status {get; set;}
}