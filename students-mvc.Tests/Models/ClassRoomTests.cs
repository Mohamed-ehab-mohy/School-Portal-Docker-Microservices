using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class ClassRoomTests
{
    [Fact]
    public void ClassRoom_DefaultCapacity_Is30()
    {
        var classroom = new ClassRoom();
        Assert.Equal(30, classroom.Capacity);
    }

    [Fact]
    public void ClassRoom_HasCorrectDefaults()
    {
        var classroom = new ClassRoom();
        Assert.Equal(string.Empty, classroom.Name);
        Assert.Equal(string.Empty, classroom.Grade);
        Assert.Equal(string.Empty, classroom.RoomNumber);
        Assert.NotNull(classroom.ClassTeachers);
        Assert.Empty(classroom.ClassTeachers);
        Assert.NotNull(classroom.StudentClasses);
        Assert.Empty(classroom.StudentClasses);
    }

    [Fact]
    public void ClassRoom_Capacity_CanBeSet()
    {
        var classroom = new ClassRoom { Capacity = 40 };
        Assert.Equal(40, classroom.Capacity);
    }
}
