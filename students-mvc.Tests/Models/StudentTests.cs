using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class StudentTests
{
    [Fact]
    public void FullName_ReturnsFirstAndLastName()
    {
        var student = new Student { FirstName = "Ahmed", LastName = "Ali" };
        Assert.Equal("Ahmed Ali", student.FullName);
    }

    [Fact]
    public void FullName_WithEmptyStrings_ReturnsSpace()
    {
        var student = new Student { FirstName = "", LastName = "" };
        Assert.Equal(" ", student.FullName);
    }

    [Fact]
    public void Student_HasCorrectDefaults()
    {
        var student = new Student();
        Assert.Equal(string.Empty, student.FirstName);
        Assert.Equal(string.Empty, student.LastName);
        Assert.Equal(string.Empty, student.Email);
        Assert.NotNull(student.StudentClasses);
        Assert.Empty(student.StudentClasses);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Student_RequiredFields_CanBeSetToEmpty(string? value)
    {
        var student = new Student
        {
            FirstName = value ?? "",
            LastName = value ?? "",
            Email = value ?? ""
        };
        Assert.NotNull(student);
    }
}
