using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Students.Any()) return;

        var firstNames = new[] { "Ahmed", "Mohamed", "Ali", "Omar", "Khaled", "Youssef", "Hassan", "Mahmoud", "Tarek", "Sami" };
        var lastNames = new[] { "Ali", "Hassan", "Ibrahim", "Saleh", "Nour", "Adel", "Karim", "Fahmy", "Saeed", "Mansour" };

        for (int i = 1; i <= 20; i++)
        {
            var firstName = firstNames[(i - 1) % firstNames.Length];
            var lastName = lastNames[(i - 1) % lastNames.Length];
            context.Students.Add(new Student
            {
                Name = $"{firstName} {lastName}",
                Email = $"student{i:00}@school.com",
                DateOfBirth = new DateTime(2000 + (i / 12), (i % 12) + 1, (i % 28) + 1),
                EnrollmentDate = new DateTime(2024, 9, 1).AddDays(i - 1)
            });
        }

        context.SaveChanges();
    }
}
