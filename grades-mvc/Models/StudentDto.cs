namespace grades_mvc.Models;

public class StudentDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
