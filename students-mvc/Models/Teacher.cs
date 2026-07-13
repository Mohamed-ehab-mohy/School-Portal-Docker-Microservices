using System.ComponentModel.DataAnnotations;

namespace students_mvc.Models;

public class Teacher
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
    [StringLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    public ICollection<ClassTeacher> ClassTeachers { get; set; } = new List<ClassTeacher>();

    public string FullName => $"{FirstName} {LastName}";
}
