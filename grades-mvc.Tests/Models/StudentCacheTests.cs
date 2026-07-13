using grades_mvc.Models;

namespace grades_mvc.Tests.Models;

public class StudentCacheTests
{
    [Fact]
    public void StudentCache_FullName_ReturnsFirstAndLastName()
    {
        var cache = new StudentCache { FirstName = "Ahmed", LastName = "Ali" };
        Assert.Equal("Ahmed Ali", cache.FullName);
    }

    [Fact]
    public void StudentCache_DefaultProperties()
    {
        var cache = new StudentCache();
        Assert.Equal(string.Empty, cache.FirstName);
        Assert.Equal(string.Empty, cache.LastName);
        Assert.Equal(string.Empty, cache.Email);
        Assert.Equal(default, cache.DateOfBirth);
        Assert.Equal(default, cache.EnrollmentDate);
        Assert.True(cache.LastUpdated <= DateTime.UtcNow);
    }

    [Fact]
    public void StudentCache_LastUpdated_DefaultsToUtcNow()
    {
        var before = DateTime.UtcNow;
        var cache = new StudentCache();
        var after = DateTime.UtcNow;

        Assert.InRange(cache.LastUpdated, before, after);
    }

    [Fact]
    public void StudentCache_FullName_WithEmptyStrings()
    {
        var cache = new StudentCache { FirstName = "", LastName = "" };
        Assert.Equal(" ", cache.FullName);
    }
}
