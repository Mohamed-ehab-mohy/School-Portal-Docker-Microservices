using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class DashboardController(
    ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalStudents = await context.Students.CountAsync();
        var totalTeachers = await context.Teachers.CountAsync();
        var totalClasses = await context.ClassRooms.CountAsync();

        var todayAttendance = await context.Attendances
            .Where(a => a.Date == today)
            .ToListAsync();

        var studentsPresentToday = todayAttendance.Count(a => a.Status == AttendanceStatus.Present);
        var studentsAbsentToday = todayAttendance.Count(a => a.Status == AttendanceStatus.Absent);
        var studentsLateToday = todayAttendance.Count(a => a.Status == AttendanceStatus.Late);
        var totalTodayAttendance = todayAttendance.Count;
        var attendanceRateToday = totalTodayAttendance > 0
            ? Math.Round((double)studentsPresentToday / totalTodayAttendance * 100, 1)
            : 0;

        var allAttendanceThisMonth = await context.Attendances
            .Where(a => a.Date >= startOfMonth && a.Date <= today)
            .ToListAsync();

        var monthlyPresent = allAttendanceThisMonth.Count(a => a.Status == AttendanceStatus.Present);
        var monthlyAbsent = allAttendanceThisMonth.Count(a => a.Status == AttendanceStatus.Absent);
        var monthlyLate = allAttendanceThisMonth.Count(a => a.Status == AttendanceStatus.Late);

        var monthlyAttendance = allAttendanceThisMonth
            .GroupBy(a => a.Date.Month)
            .Select(g => new MonthlyAttendance
            {
                Month = g.Key switch
                {
                    1 => "Jan", 2 => "Feb", 3 => "Mar", 4 => "Apr",
                    5 => "May", 6 => "Jun", 7 => "Jul", 8 => "Aug",
                    9 => "Sep", 10 => "Oct", 11 => "Nov", 12 => "Dec",
                    _ => g.Key.ToString()
                },
                Present = g.Count(a => a.Status == AttendanceStatus.Present),
                Absent = g.Count(a => a.Status == AttendanceStatus.Absent),
                Late = g.Count(a => a.Status == AttendanceStatus.Late)
            })
            .OrderBy(m => m.Month)
            .ToList();

        var classStats = await context.ClassRooms
            .Include(c => c.StudentClasses)
            .Include(c => c.ClassTeachers)
                .ThenInclude(ct => ct.Teacher)
            .ToListAsync();

        var classStatistics = classStats.Select(c =>
        {
            var studentIds = c.StudentClasses.Select(sc => sc.StudentId).ToList();
            var classAttendance = allAttendanceThisMonth
                .Where(a => studentIds.Contains(a.StudentId))
                .ToList();
            var presentCount = classAttendance.Count(a => a.Status == AttendanceStatus.Present);
            var attendanceRate = classAttendance.Count > 0
                ? Math.Round((double)presentCount / classAttendance.Count * 100, 1)
                : 0;

            return new ClassStatistics
            {
                ClassName = c.Name,
                StudentCount = c.StudentClasses.Count,
                AverageScore = 0,
                AttendanceRate = attendanceRate
            };
        }).ToList();

        var subjectStats = await context.Attendances
            .GroupBy(a => a.ClassRoom.Name)
            .Select(g => new SubjectStatistics
            {
                Subject = g.Key,
                GradeCount = g.Count(),
                AverageScore = 0
            })
            .ToListAsync();

        var teacherWorkloads = await context.Teachers
            .Include(t => t.ClassTeachers)
                .ThenInclude(ct => ct.ClassRoom)
                    .ThenInclude(c => c.StudentClasses)
            .Select(t => new TeacherWorkload
            {
                TeacherName = t.FirstName + " " + t.LastName,
                Specialization = t.Specialization,
                ClassCount = t.ClassTeachers.Count,
                StudentCount = t.ClassTeachers.SelectMany(ct => ct.ClassRoom.StudentClasses).Count()
            })
            .ToListAsync();

        var viewModel = new DashboardViewModel
        {
            TotalStudents = totalStudents,
            TotalTeachers = totalTeachers,
            TotalClasses = totalClasses,
            StudentsPresentToday = studentsPresentToday,
            StudentsAbsentToday = studentsAbsentToday,
            StudentsLateToday = studentsLateToday,
            AttendanceRateToday = attendanceRateToday,
            ClassStats = classStatistics,
            SubjectStats = subjectStats,
            MonthlyAttendance = monthlyAttendance,
            TeacherWorkloads = teacherWorkloads
        };

        return View(viewModel);
    }
}
