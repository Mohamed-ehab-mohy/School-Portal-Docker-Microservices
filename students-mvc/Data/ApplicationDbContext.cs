using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using students_mvc.Models;

namespace students_mvc.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<ClassRoom> ClassRooms { get; set; }
    public DbSet<ClassTeacher> ClassTeachers { get; set; }
    public DbSet<StudentClass> StudentClasses { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClassTeacher>()
            .HasKey(ct => new { ct.ClassRoomId, ct.TeacherId });

        modelBuilder.Entity<ClassTeacher>()
            .HasOne(ct => ct.ClassRoom)
            .WithMany(c => c.ClassTeachers)
            .HasForeignKey(ct => ct.ClassRoomId);

        modelBuilder.Entity<ClassTeacher>()
            .HasOne(ct => ct.Teacher)
            .WithMany(t => t.ClassTeachers)
            .HasForeignKey(ct => ct.TeacherId);

        modelBuilder.Entity<StudentClass>()
            .HasKey(sc => new { sc.StudentId, sc.ClassRoomId });

        modelBuilder.Entity<StudentClass>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentClasses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentClass>()
            .HasOne(sc => sc.ClassRoom)
            .WithMany(c => c.StudentClasses)
            .HasForeignKey(sc => sc.ClassRoomId);

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

        var teachers = new[]
        {
            new Teacher { Id = 1, FirstName = "Fatma", LastName = "Hassan", Email = "fatma.hassan@school.com", Specialization = "Mathematics", Phone = "01012345678", HireDate = new DateTime(2020, 9, 1) },
            new Teacher { Id = 2, FirstName = "Mahmoud", LastName = "Ali", Email = "mahmoud.ali@school.com", Specialization = "Physics", Phone = "01098765432", HireDate = new DateTime(2019, 3, 15) },
            new Teacher { Id = 3, FirstName = "Nour", LastName = "El-Din", Email = "nour.eldin@school.com", Specialization = "Arabic", Phone = "01155566677", HireDate = new DateTime(2021, 1, 10) },
            new Teacher { Id = 4, FirstName = "Sara", LastName = "Mahmoud", Email = "sara.mahmoud@school.com", Specialization = "English", Phone = "01233344455", HireDate = new DateTime(2022, 8, 20) },
            new Teacher { Id = 5, FirstName = "Khaled", LastName = "Nour", Email = "khaled.nour@school.com", Specialization = "Chemistry", Phone = "01077788899", HireDate = new DateTime(2018, 6, 1) },
        };

        modelBuilder.Entity<Teacher>().HasData(teachers);

        var classes = new[]
        {
            new ClassRoom { Id = 1, Name = "Class 1-A", Grade = "1st", RoomNumber = "Room 101", Capacity = 30 },
            new ClassRoom { Id = 2, Name = "Class 1-B", Grade = "1st", RoomNumber = "Room 102", Capacity = 30 },
            new ClassRoom { Id = 3, Name = "Class 2-A", Grade = "2nd", RoomNumber = "Room 201", Capacity = 35 },
            new ClassRoom { Id = 4, Name = "Class 2-B", Grade = "2nd", RoomNumber = "Room 202", Capacity = 35 },
            new ClassRoom { Id = 5, Name = "Class 3-A", Grade = "3rd", RoomNumber = "Room 301", Capacity = 40 },
        };

        modelBuilder.Entity<ClassRoom>().HasData(classes);

        var classTeachers = new[]
        {
            new ClassTeacher { ClassRoomId = 1, TeacherId = 1 },
            new ClassTeacher { ClassRoomId = 1, TeacherId = 3 },
            new ClassTeacher { ClassRoomId = 2, TeacherId = 2 },
            new ClassTeacher { ClassRoomId = 2, TeacherId = 4 },
            new ClassTeacher { ClassRoomId = 3, TeacherId = 1 },
            new ClassTeacher { ClassRoomId = 3, TeacherId = 5 },
            new ClassTeacher { ClassRoomId = 4, TeacherId = 3 },
            new ClassTeacher { ClassRoomId = 5, TeacherId = 2 },
            new ClassTeacher { ClassRoomId = 5, TeacherId = 4 },
        };

        modelBuilder.Entity<ClassTeacher>().HasData(classTeachers);

        var studentClasses = new[]
        {
            new StudentClass { StudentId = 1, ClassRoomId = 1, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 2, ClassRoomId = 1, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 3, ClassRoomId = 2, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 4, ClassRoomId = 2, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 5, ClassRoomId = 3, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 6, ClassRoomId = 3, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 7, ClassRoomId = 4, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 8, ClassRoomId = 4, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 9, ClassRoomId = 5, AssignedDate = new DateTime(2024, 9, 1) },
            new StudentClass { StudentId = 10, ClassRoomId = 5, AssignedDate = new DateTime(2024, 9, 1) },
        };

        modelBuilder.Entity<StudentClass>().HasData(studentClasses);
    }
}
