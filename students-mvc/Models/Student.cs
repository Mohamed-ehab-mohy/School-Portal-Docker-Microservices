using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace students_mvc.Models;

[Index(nameof(Email), IsUnique = true)]
public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }
}
