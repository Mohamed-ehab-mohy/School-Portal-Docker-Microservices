using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Controllers;
using students_mvc.Models;

namespace students_mvc.Tests.Controllers;

public class AttendanceControllerTests : IDisposable
{
    private readonly Data.ApplicationDbContext _context;
    private readonly AttendanceController _controller;

    public AttendanceControllerTests()
    {
        _context = TestDbHelper.CreateInMemoryContext(Guid.NewGuid().ToString());
        TestDbHelper.SeedTestData(_context);
        SeedAttendanceData();
        _controller = new AttendanceController(_context);
    }

    private void SeedAttendanceData()
    {
        var today = DateTime.Today;
        _context.Attendances.AddRange(
            new Attendance { StudentId = 1, ClassRoomId = 1, Date = today, Status = AttendanceStatus.Present },
            new Attendance { StudentId = 2, ClassRoomId = 1, Date = today, Status = AttendanceStatus.Absent },
            new Attendance { StudentId = 3, ClassRoomId = 2, Date = today, Status = AttendanceStatus.Late },
            new Attendance { StudentId = 4, ClassRoomId = 2, Date = today.AddDays(-1), Status = AttendanceStatus.Present }
        );
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Index_ReturnsViewWithAttendanceRecords()
    {
        var result = await _controller.Index(classId: null, date: null, search: null);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Attendance>>(viewResult.Model);
        Assert.Equal(3, model.Count);
    }

    [Fact]
    public async Task Index_WithClassFilter_FiltersByClass()
    {
        var result = await _controller.Index(classId: 1, date: null, search: null);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Attendance>>(viewResult.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task Index_WithDateFilter_FiltersByDate()
    {
        var result = await _controller.Index(classId: null, date: DateTime.Today.AddDays(-1), search: null);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Attendance>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Index_WithSearch_FiltersByStudentName()
    {
        var result = await _controller.Index(classId: null, date: null, search: "Ahmed");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Attendance>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_ExistingAttendance_ReturnsViewWithRecord()
    {
        var attendance = await _context.Attendances.FirstAsync();
        var result = await _controller.Details(attendance.Id);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Attendance>(viewResult.Model);
        Assert.Equal(attendance.Id, model.Id);
    }

    [Fact]
    public async Task Details_NullId_ReturnsNotFound()
    {
        var result = await _controller.Details(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_NonexistentId_ReturnsNotFound()
    {
        var result = await _controller.Details(9999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ExistingAttendance_RemovesFromDatabase()
    {
        var attendance = await _context.Attendances.FirstAsync();
        var countBefore = await _context.Attendances.CountAsync();

        var result = await _controller.DeleteConfirmed(attendance.Id);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore - 1, await _context.Attendances.CountAsync());
    }

    [Fact]
    public async Task DeleteConfirmed_NonexistentAttendance_Redirects()
    {
        var result = await _controller.DeleteConfirmed(9999);

        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task TakeAttendance_PostValidEntries_SavesToDatabase()
    {
        var entries = new List<AttendanceEntryViewModel>
        {
            new() { StudentId = 5, Status = AttendanceStatus.Present, Notes = null },
            new() { StudentId = 6, Status = AttendanceStatus.Absent, Notes = "Sick" }
        };
        var countBefore = await _context.Attendances.CountAsync();

        var result = await _controller.TakeAttendance(3, DateTime.Today, entries);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal(countBefore + 2, await _context.Attendances.CountAsync());
    }

    [Fact]
    public async Task TakeAttendance_EmptyEntries_DoesNotSave()
    {
        var countBefore = await _context.Attendances.CountAsync();

        var result = await _controller.TakeAttendance(1, DateTime.Today, new List<AttendanceEntryViewModel>());

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore, await _context.Attendances.CountAsync());
    }

    [Fact]
    public async Task GetAll_ReturnsJsonWithAttendanceRecords()
    {
        var result = await _controller.GetAll();

        var jsonResult = Assert.IsType<JsonResult>(result);
        var attendances = Assert.IsAssignableFrom<List<Attendance>>(jsonResult.Value);
        Assert.True(attendances.Count >= 4);
    }
}
