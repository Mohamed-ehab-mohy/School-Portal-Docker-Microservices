using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class TeacherTests
{
    [Fact]
    public void FullName_ReturnsFirstAndLastName()
    {
        var teacher = new Teacher { FirstName = "Fatma", LastName = "Hassan" };
        Assert.Equal("Fatma Hassan", teacher.FullName);
    }

    [Fact]
    public void Teacher_HasCorrectDefaults()
    {
        var teacher = new Teacher();
        Assert.Equal(string.Empty, teacher.FirstName);
        Assert.Equal(string.Empty, teacher.LastName);
        Assert.Equal(string.Empty, teacher.Email);
        Assert.Equal(string.Empty, teacher.Specialization);
        Assert.Equal(string.Empty, teacher.Phone);
        Assert.NotNull(teacher.ClassTeachers);
        Assert.Empty(teacher.ClassTeachers);
    }

    [Fact]
    public void Teacher_Specialization_CanBeSet()
    {
        var teacher = new Teacher { Specialization = "Mathematics" };
        Assert.Equal("Mathematics", teacher.Specialization);
    }

    [Fact]
    public void Teacher_HireDate_CanBeSet()
    {
        var date = new DateTime(2020, 9, 1);
        var teacher = new Teacher { HireDate = date };
        Assert.Equal(date, teacher.HireDate);
    }
}
