using System.ComponentModel.DataAnnotations;

namespace students_mvc.Models;

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    Excused
}

public class Attendance
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [Required]
    public int ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required]
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    [StringLength(200)]
    public string? Notes { get; set; }
}
