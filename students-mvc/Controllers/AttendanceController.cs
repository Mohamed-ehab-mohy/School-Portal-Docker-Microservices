using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;
using students_mvc.Services;

namespace students_mvc.Controllers;

[Authorize]
public class AttendanceController(ApplicationDbContext context, INotificationService notificationService) : Controller
{
    public async Task<IActionResult> Index(int? classId, DateTime? date, string? search, int page = 1)
    {
        const int pageSize = 15;
        var selectedDate = date ?? DateTime.Today;
        var selectedClassId = classId ?? 0;

        var query = context.Attendances
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .AsQueryable();

        if (selectedClassId > 0)
        {
            query = query.Where(a => a.ClassRoomId == selectedClassId);
        }

        query = query.Where(a => a.Date == selectedDate);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(a =>
                a.Student.FirstName.ToLower().Contains(searchLower) ||
                a.Student.LastName.ToLower().Contains(searchLower) ||
                a.ClassRoom.Name.ToLower().Contains(searchLower));
        }

        query = query.OrderBy(a => a.Student.LastName).ThenBy(a => a.Student.FirstName);
        var attendances = await PaginatedList<Attendance>.CreateAsync(query, page, pageSize, search);

        await PopulateClassDropdownAsync();
        ViewBag.SelectedClassId = selectedClassId;
        ViewBag.SelectedDate = selectedDate;
        ViewBag.SearchTerm = search;
        ViewBag.Page = page;

        return View(attendances);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var attendance = await context.Attendances
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (attendance is null)
        {
            return NotFound();
        }

        return View(attendance);
    }

    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> TakeAttendance(int? classId, DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;
        var selectedClassId = classId ?? 0;

        await PopulateClassDropdownAsync();

        if (selectedClassId > 0)
        {
            var students = await context.StudentClasses
                .Where(sc => sc.ClassRoomId == selectedClassId)
                .Include(sc => sc.Student)
                .Select(sc => sc.Student)
                .ToListAsync();

            var existingAttendance = await context.Attendances
                .Where(a => a.ClassRoomId == selectedClassId && a.Date == selectedDate)
                .ToListAsync();

            var viewModelList = students.Select(s =>
            {
                var existing = existingAttendance.FirstOrDefault(a => a.StudentId == s.Id);
                return new AttendanceEntryViewModel
                {
                    StudentId = s.Id,
                    StudentName = s.FullName,
                    Status = existing?.Status ?? AttendanceStatus.Present,
                    Notes = existing?.Notes
                };
            }).ToList();

            ViewBag.SelectedClassId = selectedClassId;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.ClassName = (await context.ClassRooms.FindAsync(selectedClassId))?.Name;

            return View(viewModelList);
        }

        ViewBag.SelectedClassId = 0;
        ViewBag.SelectedDate = selectedDate;
        return View(new List<AttendanceEntryViewModel>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> TakeAttendance(int classId, DateTime date, List<AttendanceEntryViewModel> entries)
    {
        if (entries is { Count: > 0 })
        {
            var existingAttendance = await context.Attendances
                .Where(a => a.ClassRoomId == classId && a.Date == date)
                .ToListAsync();

            context.Attendances.RemoveRange(existingAttendance);

            foreach (var entry in entries)
            {
                context.Attendances.Add(new Attendance
                {
                    StudentId = entry.StudentId,
                    ClassRoomId = classId,
                    Date = date,
                    Status = entry.Status,
                    Notes = entry.Notes
                });
            }

            await context.SaveChangesAsync();

            var absentCount = entries.Count(e => e.Status == AttendanceStatus.Absent);
            var lateCount = entries.Count(e => e.Status == AttendanceStatus.Late);
            var className = (await context.ClassRooms.FindAsync(classId))?.Name ?? "Unknown";
            var dateStr = date.ToString("MMM dd, yyyy");

            if (absentCount > 0)
            {
                await notificationService.CreateAsync(
                    "Absences Recorded",
                    $"{absentCount} student{(absentCount > 1 ? "s" : "")} marked absent in {className} on {dateStr}.",
                    NotificationType.Warning, NotificationCategory.Attendance,
                    $"/Attendance?classId={classId}&date={date:yyyy-MM-dd}");
            }

            if (lateCount > 0)
            {
                await notificationService.CreateAsync(
                    "Late Arrivals Recorded",
                    $"{lateCount} student{(lateCount > 1 ? "s" : "")} marked late in {className} on {dateStr}.",
                    NotificationType.Info, NotificationCategory.Attendance,
                    $"/Attendance?classId={classId}&date={date:yyyy-MM-dd}");
            }

            if (absentCount == 0 && lateCount == 0)
            {
                await notificationService.CreateAsync(
                    "Attendance Completed",
                    $"All {entries.Count} students present in {className} on {dateStr}.",
                    NotificationType.Success, NotificationCategory.Attendance,
                    $"/Attendance?classId={classId}&date={date:yyyy-MM-dd}");
            }
        }

        return RedirectToAction(nameof(Index), new { classId, date = date.ToString("yyyy-MM-dd") });
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var attendance = await context.Attendances
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (attendance is null)
        {
            return NotFound();
        }

        return View(attendance);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var attendance = await context.Attendances.FindAsync(id);
        if (attendance is not null)
        {
            context.Attendances.Remove(attendance);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/attendance")]
    public async Task<JsonResult> GetAll()
    {
        var attendances = await context.Attendances
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .ToListAsync();
        return Json(attendances);
    }

    private async Task PopulateClassDropdownAsync()
    {
        var classes = await context.ClassRooms.ToListAsync();
        ViewBag.ClassList = new SelectList(classes, "Id", "Name");
    }
}

public class AttendanceEntryViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Notes { get; set; }
}
