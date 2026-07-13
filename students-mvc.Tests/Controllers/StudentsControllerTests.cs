using Microsoft.AspNetCore.Mvc;
using Moq;
using students_mvc.Controllers;
using students_mvc.Messaging;
using students_mvc.Models;

namespace students_mvc.Tests.Controllers;

public class StudentsControllerTests : IDisposable
{
    private readonly Data.ApplicationDbContext _context;
    private readonly Mock<IStudentMessageBus> _messageBusMock;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _context = TestDbHelper.CreateInMemoryContext(Guid.NewGuid().ToString());
        TestDbHelper.SeedTestData(_context);
        _messageBusMock = new Mock<IStudentMessageBus>();
        _controller = new StudentsController(_context, _messageBusMock.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private int GetStudentCount() => _context.Students.Count();

    [Fact]
    public async Task Details_ExistingStudent_ReturnsViewWithStudent()
    {
        var result = await _controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.Model);
        Assert.Equal(1, model.Id);
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
    public async Task GetAll_ReturnsJsonWithStudents()
    {
        var result = await _controller.GetAll();

        var jsonResult = Assert.IsType<JsonResult>(result);
        var students = Assert.IsAssignableFrom<List<Student>>(jsonResult.Value);
        Assert.True(students.Count >= 20);
    }

    [Fact]
    public async Task GetById_ExistingStudent_ReturnsJson()
    {
        var result = await _controller.GetById(1);

        var jsonResult = Assert.IsType<JsonResult>(result);
        var student = Assert.IsType<Student>(jsonResult.Value);
        Assert.Equal(1, student.Id);
    }

    [Fact]
    public async Task GetById_NonexistentStudent_ReturnsJsonNull()
    {
        var result = await _controller.GetById(9999);

        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Null(jsonResult.Value);
    }

    [Fact]
    public async Task Create_PostValidStudent_SavesAndPublishesEvent()
    {
        var countBefore = GetStudentCount();
        var student = new Student
        {
            FirstName = "New",
            LastName = "Student",
            Email = "new@test.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            EnrollmentDate = DateTime.Today
        };

        var result = await _controller.Create(student);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore + 1, GetStudentCount());
        _messageBusMock.Verify(m =>
            m.PublishStudentEvent(It.IsAny<Student>(), StudentEventTypes.Created),
            Times.Once);
    }

    [Fact]
    public async Task Create_PostInvalidModel_ReturnsViewWithModel()
    {
        _controller.ModelState.AddModelError("FirstName", "Required");
        var student = new Student();

        var result = await _controller.Create(student);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<Student>(viewResult.Model);
    }

    [Fact]
    public async Task Edit_PostValidStudent_UpdatesAndPublishesEvent()
    {
        var student = (await _context.Students.FindAsync(1))!;
        student.FirstName = "Updated";

        var result = await _controller.Edit(1, student);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Updated", (await _context.Students.FindAsync(1))!.FirstName);
        _messageBusMock.Verify(m =>
            m.PublishStudentEvent(It.IsAny<Student>(), StudentEventTypes.Updated),
            Times.Once);
    }

    [Fact]
    public async Task Edit_IdMismatch_ReturnsNotFound()
    {
        var student = new Student { Id = 2 };

        var result = await _controller.Edit(1, student);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ExistingStudent_RemovesAndPublishesEvent()
    {
        var result = await _controller.DeleteConfirmed(1);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(await _context.Students.FindAsync(1));
        _messageBusMock.Verify(m =>
            m.PublishStudentEvent(It.IsAny<Student>(), StudentEventTypes.Deleted),
            Times.Once);
    }

    [Fact]
    public async Task DeleteConfirmed_NonexistentStudent_Redirects()
    {
        var result = await _controller.DeleteConfirmed(9999);

        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Details_DeleteView_ReturnsViewWithStudent()
    {
        var result = await _controller.Delete(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.Model);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public async Task Delete_NullId_ReturnsNotFound()
    {
        var result = await _controller.Delete(null);

        Assert.IsType<NotFoundResult>(result);
    }
}
