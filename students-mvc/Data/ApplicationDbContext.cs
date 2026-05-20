using Microsoft.EntityFrameworkCore;
using students_mvc.Models;

namespace students_mvc.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var firstNames = new[] { "Ahmed", "Mohamed", "Ali", "Omar", "Khaled", "Youssef", "Hassan", "Mahmoud", "Tarek", "Sami" };
        var lastNames = new[] { "Ali", "Hassan", "Ibrahim", "Saleh", "Nour", "Adel", "Karim", "Fahmy", "Saeed", "Mansour" };

        var students = new List<Student>();
        for (int i = 1; i <= 20; i++)
        {
            students.Add(new Student
            {
                Id = i,
                FirstName = firstNames[(i - 1) % firstNames.Length],
                LastName = lastNames[(i - 1) % lastNames.Length],
                Email = $"student{i:00}@school.com",
                DateOfBirth = new DateTime(2000 + (i / 12), (i % 12) + 1, (i % 28) + 1),
                EnrollmentDate = new DateTime(2024, 9, 1).AddDays(i - 1)
            });
        }

        modelBuilder.Entity<Student>().HasData(students);
    }
}
