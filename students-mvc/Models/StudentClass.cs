namespace students_mvc.Models;

public class StudentClass
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;

    public DateTime AssignedDate { get; set; } = DateTime.Today;
}
