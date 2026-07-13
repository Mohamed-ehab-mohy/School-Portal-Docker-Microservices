using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class AttendanceTests
{
    [Fact]
    public void Attendance_DefaultStatus_IsPresent()
    {
        var attendance = new Attendance();
        Assert.Equal(AttendanceStatus.Present, attendance.Status);
    }

    [Fact]
    public void Attendance_DefaultDate_IsToday()
    {
        var attendance = new Attendance();
        Assert.Equal(DateTime.Today, attendance.Date);
    }

    [Fact]
    public void Attendance_Notes_IsOptional()
    {
        var attendance = new Attendance { Notes = null };
        Assert.Null(attendance.Notes);
    }

    [Fact]
    public void Attendance_AllStatuses_AreDefined()
    {
        Assert.Equal(0, (int)AttendanceStatus.Present);
        Assert.Equal(1, (int)AttendanceStatus.Absent);
        Assert.Equal(2, (int)AttendanceStatus.Late);
        Assert.Equal(3, (int)AttendanceStatus.Excused);
    }

    [Theory]
    [InlineData(AttendanceStatus.Present)]
    [InlineData(AttendanceStatus.Absent)]
    [InlineData(AttendanceStatus.Late)]
    [InlineData(AttendanceStatus.Excused)]
    public void Attendance_Status_CanBeSetToAnyValue(AttendanceStatus status)
    {
        var attendance = new Attendance { Status = status };
        Assert.Equal(status, attendance.Status);
    }
}
