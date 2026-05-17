using grades_mvc.Data;
using grades_mvc.Models;

namespace grades_mvc;

public static class DbInitializer
{
    public static void Seed(GradesDbContext context)
    {
        if (context.Grades.Any()) return;

        var subjects = new[] { "Mathematics", "Science", "English" };
        var random = new Random(42);

        for (int i = 1; i <= 20; i++)
        {
            context.Grades.Add(new Grade
            {
                StudentId = i,
                Subject = subjects[(i - 1) % subjects.Length],
                Score = random.Next(50, 100),
                Date = new DateTime(2025, 9, 1).AddDays(i - 1)
            });
        }

        context.SaveChanges();
    }
}
