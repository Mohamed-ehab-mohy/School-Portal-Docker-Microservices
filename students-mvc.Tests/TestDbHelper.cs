using Microsoft.EntityFrameworkCore;
using students_mvc.Data;

namespace students_mvc.Tests;

public static class TestDbHelper
{
    public static ApplicationDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static void SeedTestData(ApplicationDbContext context)
    {
        if (context.Students.Any()) return;

        for (int i = 21; i <= 45; i++)
        {
            context.Students.Add(new students_mvc.Models.Student
            {
                Id = i,
                FirstName = $"Test{i}",
                LastName = $"Student{i}",
                Email = $"teststudent{i}@test.com",
                DateOfBirth = new DateTime(2000, 1, 1),
                EnrollmentDate = new DateTime(2024, 9, 1)
            });
        }

        context.SaveChanges();
    }
}
