namespace grades_mvc.Messaging;

public class StudentEvent
{
    public string EventType { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime Timestamp { get; set; }
}
