using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using grades_mvc.Controllers;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using grades_mvc.ViewModels;

namespace grades_mvc.Tests.Controllers;

public class GradesControllerTests : IDisposable
{
    private readonly GradesDbContext _context;
    private readonly Mock<IStudentsServiceClient> _studentsClientMock;
    private readonly GradesController _controller;

    public GradesControllerTests()
    {
        _context = GradesTestDbHelper.CreateInMemoryContext(Guid.NewGuid().ToString());
        GradesTestDbHelper.SeedTestData(_context);
        _studentsClientMock = new Mock<IStudentsServiceClient>();
        SetupStudentsClientMock();
        _controller = new GradesController(_context, _studentsClientMock.Object);
    }

    private void SetupStudentsClientMock()
    {
        var students = new List<Student>
        {
            new() { Id = 1, FirstName = "Ahmed", LastName = "Ali", Email = "ahmed@test.com" },
            new() { Id = 2, FirstName = "Mohamed", LastName = "Hassan", Email = "mohamed@test.com" }
        };

        _studentsClientMock
            .Setup(c => c.GetAllStudentsAsync())
            .ReturnsAsync(students);

        _studentsClientMock
            .Setup(c => c.GetStudentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => students.FirstOrDefault(s => s.Id == id));

        _studentsClientMock
            .Setup(c => c.GetStudentsByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((IEnumerable<int> ids) =>
                students.Where(s => ids.Contains(s.Id)).ToDictionary(s => s.Id));
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Index_ReturnsViewWithGrades()
    {
        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<GradeViewModel>>(viewResult.Model);
        Assert.Equal(20, model.Count);
    }

    [Fact]
    public async Task Details_ExistingGrade_ReturnsViewWithGrade()
    {
        var result = await _controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<GradeViewModel>(viewResult.Model);
        Assert.Equal(1, model.Id);
        Assert.Equal("Ahmed Ali", model.StudentName);
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
    public async Task Create_PostValidGrade_SavesToDatabase()
    {
        var viewModel = new GradeFormViewModel
        {
            StudentId = 1,
            CourseName = "History",
            Score = 88,
            GradeDate = DateTime.Today,
            Notes = "Good"
        };
        var countBefore = _context.Grades.Count();

        var result = await _controller.Create(viewModel);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore + 1, _context.Grades.Count());
    }

    [Fact]
    public async Task Create_PostInvalidModel_ReturnsViewWithModel()
    {
        _controller.ModelState.AddModelError("CourseName", "Required");
        var viewModel = new GradeFormViewModel();

        var result = await _controller.Create(viewModel);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<GradeFormViewModel>(viewResult.Model);
    }

    [Fact]
    public async Task Edit_PostValidGrade_UpdatesInDatabase()
    {
        var viewModel = new GradeFormViewModel
        {
            Id = 1,
            StudentId = 1,
            CourseName = "Updated Math",
            Score = 95,
            GradeDate = DateTime.Today,
            Notes = "Updated"
        };

        var result = await _controller.Edit(1, viewModel);

        Assert.IsType<RedirectToActionResult>(result);
        var grade = await _context.Grades.FindAsync(1);
        Assert.Equal("Updated Math", grade!.CourseName);
        Assert.Equal(95, grade.Score);
    }

    [Fact]
    public async Task Edit_IdMismatch_ReturnsNotFound()
    {
        var viewModel = new GradeFormViewModel { Id = 2 };

        var result = await _controller.Edit(1, viewModel);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_NonexistentGrade_ReturnsNotFound()
    {
        var viewModel = new GradeFormViewModel
        {
            Id = 9999,
            StudentId = 1,
            CourseName = "X",
            Score = 50,
            GradeDate = DateTime.Today
        };

        var result = await _controller.Edit(9999, viewModel);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ExistingGrade_RemovesFromDatabase()
    {
        var result = await _controller.DeleteConfirmed(1);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(await _context.Grades.FindAsync(1));
    }

    [Fact]
    public async Task DeleteConfirmed_NonexistentGrade_Redirects()
    {
        var countBefore = _context.Grades.Count();

        var result = await _controller.DeleteConfirmed(9999);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore, _context.Grades.Count());
    }

    [Fact]
    public async Task Index_UsesStudentClientToResolveNames()
    {
        await _controller.Index();

        _studentsClientMock.Verify(
            c => c.GetStudentsByIdsAsync(It.IsAny<IEnumerable<int>>()),
            Times.Once);
    }
}
