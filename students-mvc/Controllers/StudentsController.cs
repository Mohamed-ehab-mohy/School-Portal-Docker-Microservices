using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Messaging;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class StudentsController(
    ApplicationDbContext context,
    IStudentMessageBus messageBus) : Controller
{
    [HttpGet("api/students")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<JsonResult> GetAll()
    {
        var students = await context.Students.ToListAsync();
        return Json(students);
    }

    [HttpGet("api/students/{id:int}")]
    public async Task<JsonResult> GetById(int id)
    {
        var student = await context.Students.FindAsync(id);
        return Json(student);
    }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        if (User.IsInRole("Admin") || User.IsInRole("Teacher"))
        {
            const int pageSize = 10;
            var query = context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(searchLower) ||
                    s.LastName.ToLower().Contains(searchLower) ||
                    s.Email.ToLower().Contains(searchLower));
            }

            query = query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
            var students = await PaginatedList<Student>.CreateAsync(query, page, pageSize, search);
            return View("Index", students);
        }

        var email = User.FindFirstValue(ClaimTypes.Email);
        var student = await context.Students.FirstOrDefaultAsync(s => s.Email == email);
        if (student is not null)
        {
            return View("Details", student);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [Authorize(Roles = "Admin,Teacher")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,DateOfBirth,EnrollmentDate")] Student student)
    {
        if (ModelState.IsValid)
        {
            context.Add(student);
            await context.SaveChangesAsync();
            await messageBus.PublishStudentEvent(student, StudentEventTypes.Created);
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,DateOfBirth,EnrollmentDate")] Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(student);
                await context.SaveChangesAsync();
                await messageBus.PublishStudentEvent(student, StudentEventTypes.Updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExists(student.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await context.Students.FindAsync(id);
        if (student is not null)
        {
            await messageBus.PublishStudentEvent(student, StudentEventTypes.Deleted);
            context.Students.Remove(student);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private Task<bool> StudentExists(int id)
    {
        return context.Students.AnyAsync(e => e.Id == id);
    }
}
