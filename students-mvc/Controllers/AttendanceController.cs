using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class AttendanceController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(int? classId, DateTime? date)
    {
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

        var attendances = await query.OrderBy(a => a.Student.LastName).ToListAsync();

        await PopulateClassDropdownAsync();
        ViewBag.SelectedClassId = selectedClassId;
        ViewBag.SelectedDate = selectedDate;

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
