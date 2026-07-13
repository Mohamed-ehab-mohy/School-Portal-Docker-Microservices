using Microsoft.EntityFrameworkCore;
using grades_mvc.Data;
using grades_mvc.Models;

namespace grades_mvc.Tests;

public static class GradesTestDbHelper
{
    public static GradesDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<GradesDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new GradesDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static void SeedTestData(GradesDbContext context)
    {
        if (context.Grades.Any()) return;

        context.Grades.AddRange(
            new Grade { Id = 1, StudentId = 1, CourseName = "Mathematics", Score = 85, GradeDate = new DateTime(2025, 9, 1) },
            new Grade { Id = 2, StudentId = 2, CourseName = "Science", Score = 92, GradeDate = new DateTime(2025, 9, 2) },
            new Grade { Id = 3, StudentId = 1, CourseName = "English", Score = 78, GradeDate = new DateTime(2025, 9, 3) }
        );

        context.StudentCache.AddRange(
            new StudentCache { Id = 1, FirstName = "Ahmed", LastName = "Ali", Email = "ahmed@test.com" },
            new StudentCache { Id = 2, FirstName = "Mohamed", LastName = "Hassan", Email = "mohamed@test.com" }
        );

        context.SaveChanges();
    }
}
