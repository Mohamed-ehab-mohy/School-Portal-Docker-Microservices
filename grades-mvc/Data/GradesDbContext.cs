using grades_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Data;

public class GradesDbContext(DbContextOptions<GradesDbContext> options) : DbContext(options)
{
    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Grade>()
            .Property(grade => grade.Score)
            .HasPrecision(5, 2);

        var subjects = new[] { "Mathematics", "Science", "English" };
        var random = new Random(42);

        var grades = new List<Grade>();
        for (int i = 1; i <= 20; i++)
        {
            grades.Add(new Grade
            {
                Id = i,
                StudentId = i,
                CourseName = subjects[(i - 1) % subjects.Length],
                Score = random.Next(50, 100),
                Date = new DateTime(2025, 9, 1).AddDays(i - 1)
            });
        }

        modelBuilder.Entity<Grade>().HasData(grades);
    }
}
