using grades_mvc.Models;

namespace grades_mvc.Tests.Models;

public class GradeTests
{
    [Fact]
    public void Grade_DefaultProperties()
    {
        var grade = new Grade();
        Assert.Equal(string.Empty, grade.CourseName);
        Assert.Null(grade.Notes);
    }

    [Fact]
    public void Grade_Score_CanBeSet()
    {
        var grade = new Grade { Score = 85.5m };
        Assert.Equal(85.5m, grade.Score);
    }

    [Fact]
    public void Grade_Notes_IsOptional()
    {
        var grade = new Grade { Notes = null };
        Assert.Null(grade.Notes);
    }

    [Fact]
    public void Grade_Notes_CanBeSet()
    {
        var grade = new Grade { Notes = "Good performance" };
        Assert.Equal("Good performance", grade.Notes);
    }

    [Fact]
    public void Grade_StudentId_CanBeSet()
    {
        var grade = new Grade { StudentId = 42 };
        Assert.Equal(42, grade.StudentId);
    }

    [Fact]
    public void Grade_GradeDate_CanBeSet()
    {
        var date = new DateTime(2025, 6, 15);
        var grade = new Grade { GradeDate = date };
        Assert.Equal(date, grade.GradeDate);
    }
}
