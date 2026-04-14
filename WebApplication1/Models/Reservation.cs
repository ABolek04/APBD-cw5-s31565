namespace WebApplication1.Models;

public class Reservation
{
    public int Id {get; set;}
    //Foreign key
    public int RoomId {get; set;}
    public string OrganizerName {get; set;}
    public string Topic { get; set; }=string.Empty;
    public string StartTime {get; set;}
    public string EndTime {get; set;}
    public string status {get; set;}
}