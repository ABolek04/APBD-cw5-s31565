using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Reservation : IValidatableObject
{
    public int Id {get; set;}
    //Foreign key
    public int RoomId {get; set;}
    [Required]
    public string OrganizerName {get; set;}
    [Required]
    public string Topic { get; set; }
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}
    public string status {get; set;}

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(errorMessage: "End Date musi być pozniej niz Start Date");
        }
    }
    
}