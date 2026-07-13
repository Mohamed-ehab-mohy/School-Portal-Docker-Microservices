using System.ComponentModel.DataAnnotations;

namespace students_mvc.Models;

public class ClassRoom
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Grade { get; set; } = string.Empty;

    [StringLength(200)]
    public string RoomNumber { get; set; } = string.Empty;

    [Range(1, 50)]
    public int Capacity { get; set; } = 30;

    public ICollection<ClassTeacher> ClassTeachers { get; set; } = new List<ClassTeacher>();

    public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}
