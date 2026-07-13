namespace students_mvc.Models;

public class ClassTeacher
{
    public int ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
}
