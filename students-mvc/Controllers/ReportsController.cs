using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class ReportsController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> StudentPerformance(int? studentId)
    {
        var students = await context.Students
            .Include(s => s.StudentClasses)
                .ThenInclude(sc => sc.ClassRoom)
            .ToListAsync();

        ViewBag.Students = students;

        if (studentId.HasValue)
        {
            var student = await context.Students
                .Include(s => s.StudentClasses)
                    .ThenInclude(sc => sc.ClassRoom)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student is not null)
            {
                var attendance = await context.Attendances
                    .Where(a => a.StudentId == studentId)
                    .ToListAsync();

                var totalDays = attendance.Count;
                var presentDays = attendance.Count(a => a.Status == AttendanceStatus.Present);
                var absentDays = attendance.Count(a => a.Status == AttendanceStatus.Absent);
                var lateDays = attendance.Count(a => a.Status == AttendanceStatus.Late);

                var viewModel = new StudentPerformanceReport
                {
                    Student = student,
                    TotalDays = totalDays,
                    PresentDays = presentDays,
                    AbsentDays = absentDays,
                    LateDays = lateDays,
                    AttendanceRate = totalDays > 0 ? Math.Round((double)presentDays / totalDays * 100, 1) : 0,
                    Classes = student.StudentClasses.Select(sc => sc.ClassRoom.Name).ToList()
                };

                return View(viewModel);
            }
        }

        return View(new StudentPerformanceReport());
    }

    public async Task<IActionResult> AttendanceReport(DateTime? startDate, DateTime? endDate, int? classId)
    {
        var start = startDate ?? DateTime.Today.AddDays(-30);
        var end = endDate ?? DateTime.Today;

        var query = context.Attendances
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .Where(a => a.Date >= start && a.Date <= end)
            .AsQueryable();

        if (classId.HasValue)
        {
            query = query.Where(a => a.ClassRoomId == classId);
        }

        var attendances = await query.ToListAsync();

        var summary = new AttendanceReportSummary
        {
            StartDate = start,
            EndDate = end,
            TotalRecords = attendances.Count,
            PresentCount = attendances.Count(a => a.Status == AttendanceStatus.Present),
            AbsentCount = attendances.Count(a => a.Status == AttendanceStatus.Absent),
            LateCount = attendances.Count(a => a.Status == AttendanceStatus.Late),
            ExcusedCount = attendances.Count(a => a.Status == AttendanceStatus.Excused),
            DailyBreakdown = attendances
                .GroupBy(a => a.Date)
                .Select(g => new DailyAttendance
                {
                    Date = g.Key,
                    Present = g.Count(a => a.Status == AttendanceStatus.Present),
                    Absent = g.Count(a => a.Status == AttendanceStatus.Absent),
                    Late = g.Count(a => a.Status == AttendanceStatus.Late),
                    Excused = g.Count(a => a.Status == AttendanceStatus.Excused)
                })
                .OrderBy(d => d.Date)
                .ToList()
        };

        var classes = await context.ClassRooms.ToListAsync();
        ViewBag.Classes = classes;
        ViewBag.SelectedClassId = classId;
        ViewBag.StartDate = start;
        ViewBag.EndDate = end;

        return View(summary);
    }

    public async Task<IActionResult> ClassReport()
    {
        var classes = await context.ClassRooms
            .Include(c => c.StudentClasses)
                .ThenInclude(sc => sc.Student)
            .Include(c => c.ClassTeachers)
                .ThenInclude(ct => ct.Teacher)
            .ToListAsync();

        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        var monthlyAttendance = await context.Attendances
            .Where(a => a.Date >= startOfMonth)
            .ToListAsync();

        var reports = classes.Select(c =>
        {
            var studentIds = c.StudentClasses.Select(sc => sc.StudentId).ToList();
            var classAttendance = monthlyAttendance
                .Where(a => studentIds.Contains(a.StudentId))
                .ToList();

            var presentCount = classAttendance.Count(a => a.Status == AttendanceStatus.Present);
            var attendanceRate = classAttendance.Count > 0
                ? Math.Round((double)presentCount / classAttendance.Count * 100, 1)
                : 0;

            return new ClassReportViewModel
            {
                ClassName = c.Name,
                Grade = c.Grade,
                RoomNumber = c.RoomNumber,
                Capacity = c.Capacity,
                StudentCount = c.StudentClasses.Count,
                Teachers = c.ClassTeachers.Select(ct => ct.Teacher.FullName).ToList(),
                AttendanceRate = attendanceRate,
                Students = c.StudentClasses.Select(sc => new StudentSummary
                {
                    Name = sc.Student.FullName,
                    Email = sc.Student.Email
                }).ToList()
            };
        }).ToList();

        return View(reports);
    }
}

public class StudentPerformanceReport
{
    public Student Student { get; set; } = null!;
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public double AttendanceRate { get; set; }
    public List<string> Classes { get; set; } = [];
}

public class AttendanceReportSummary
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalRecords { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int LateCount { get; set; }
    public int ExcusedCount { get; set; }
    public List<DailyAttendance> DailyBreakdown { get; set; } = [];
}

public class DailyAttendance
{
    public DateTime Date { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Late { get; set; }
    public int Excused { get; set; }
}

public class ClassReportViewModel
{
    public string ClassName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int StudentCount { get; set; }
    public List<string> Teachers { get; set; } = [];
    public double AttendanceRate { get; set; }
    public List<StudentSummary> Students { get; set; } = [];
}

public class StudentSummary
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
