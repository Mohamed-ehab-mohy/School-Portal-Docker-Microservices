using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Controllers;
using students_mvc.Models;

namespace students_mvc.Tests.Controllers;

public class TeachersControllerTests : IDisposable
{
    private readonly Data.ApplicationDbContext _context;
    private readonly TeachersController _controller;

    public TeachersControllerTests()
    {
        _context = TestDbHelper.CreateInMemoryContext(Guid.NewGuid().ToString());
        TestDbHelper.SeedTestData(_context);
        _controller = new TeachersController(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Index_ReturnsViewWithTeachers()
    {
        var result = await _controller.Index(search: null);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Teacher>>(viewResult.Model);
        Assert.Equal(5, model.Count);
    }

    [Fact]
    public async Task Index_WithSearch_FiltersTeachersByName()
    {
        var result = await _controller.Index(search: "Fatma");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Teacher>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Fatma", model[0].FirstName);
    }

    [Fact]
    public async Task Index_WithSearchBySpecialization_FiltersTeachers()
    {
        var result = await _controller.Index(search: "Physics");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Teacher>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Physics", model[0].Specialization);
    }

    [Fact]
    public async Task Index_EmptySearch_ReturnsAllTeachers()
    {
        var result = await _controller.Index(search: "");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Teacher>>(viewResult.Model);
        Assert.Equal(5, model.Count);
    }

    [Fact]
    public async Task Details_ExistingTeacher_ReturnsViewWithTeacher()
    {
        var result = await _controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Teacher>(viewResult.Model);
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
    public async Task Create_PostValidTeacher_SavesToDatabase()
    {
        var countBefore = await _context.Teachers.CountAsync();
        var teacher = new Teacher
        {
            FirstName = "New",
            LastName = "Teacher",
            Email = "new@teacher.com",
            Specialization = "Physics",
            Phone = "01012345678",
            HireDate = DateTime.Today
        };

        var result = await _controller.Create(teacher);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(countBefore + 1, await _context.Teachers.CountAsync());
    }

    [Fact]
    public async Task Create_PostInvalidModel_ReturnsViewWithModel()
    {
        _controller.ModelState.AddModelError("FirstName", "Required");
        var teacher = new Teacher();

        var result = await _controller.Create(teacher);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<Teacher>(viewResult.Model);
    }

    [Fact]
    public async Task Edit_PostValidTeacher_UpdatesInDatabase()
    {
        var teacher = (await _context.Teachers.FindAsync(1))!;
        teacher.FirstName = "Updated";

        var result = await _controller.Edit(1, teacher);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Updated", (await _context.Teachers.FindAsync(1))!.FirstName);
    }

    [Fact]
    public async Task Edit_IdMismatch_ReturnsNotFound()
    {
        var teacher = new Teacher { Id = 2 };

        var result = await _controller.Edit(1, teacher);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ExistingTeacher_RemovesFromDatabase()
    {
        var result = await _controller.DeleteConfirmed(1);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(await _context.Teachers.FindAsync(1));
    }

    [Fact]
    public async Task DeleteConfirmed_NonexistentTeacher_Redirects()
    {
        var result = await _controller.DeleteConfirmed(9999);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(5, await _context.Teachers.CountAsync());
    }

    [Fact]
    public async Task GetAll_ReturnsJsonWithTeachers()
    {
        var result = await _controller.GetAll();

        var jsonResult = Assert.IsType<JsonResult>(result);
        var teachers = Assert.IsAssignableFrom<List<Teacher>>(jsonResult.Value);
        Assert.Equal(5, teachers.Count);
    }

    [Fact]
    public async Task GetById_ExistingTeacher_ReturnsJson()
    {
        var result = await _controller.GetById(1);

        var jsonResult = Assert.IsType<JsonResult>(result);
        var teacher = Assert.IsType<Teacher>(jsonResult.Value);
        Assert.Equal(1, teacher.Id);
    }

    [Fact]
    public async Task GetById_NonexistentTeacher_ReturnsJsonNull()
    {
        var result = await _controller.GetById(9999);

        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Null(jsonResult.Value);
    }

    [Fact]
    public async Task Edit_NonexistentTeacher_ReturnsNotFound()
    {
        var teacher = new Teacher { Id = 9999, FirstName = "X", LastName = "Y", Email = "x@y.com", Specialization = "Z", Phone = "01012345678", HireDate = DateTime.Today };

        var result = await _controller.Edit(9999, teacher);

        Assert.IsType<NotFoundResult>(result);
    }
}
