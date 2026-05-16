using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

public class StudentsController(ApplicationDbContext context) : Controller
{
    [HttpGet("api/students")]
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

    public async Task<IActionResult> Index()
    {
        return View(await context.Students.ToListAsync());
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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Email,DateOfBirth,EnrollmentDate")] Student student)
    {
        if (ModelState.IsValid)
        {
            context.Add(student);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,DateOfBirth,EnrollmentDate")] Student student)
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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await context.Students.FindAsync(id);
        if (student is not null)
        {
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
