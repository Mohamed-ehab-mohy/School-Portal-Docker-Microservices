using System.ComponentModel.DataAnnotations;

namespace grades_mvc.Models;

public class StudentCache
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}";
}
