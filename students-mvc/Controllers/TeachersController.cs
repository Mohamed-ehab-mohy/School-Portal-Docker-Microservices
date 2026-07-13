using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class TeachersController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Teachers.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher is null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Specialization,Phone,HireDate")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            context.Add(teacher);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(teacher);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers.FindAsync(id);
        if (teacher is null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Specialization,Phone,HireDate")] Teacher teacher)
    {
        if (id != teacher.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(teacher);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TeacherExists(teacher.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(teacher);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher is null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await context.Teachers.FindAsync(id);
        if (teacher is not null)
        {
            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/teachers")]
    public async Task<JsonResult> GetAll()
    {
        var teachers = await context.Teachers.ToListAsync();
        return Json(teachers);
    }

    [HttpGet("api/teachers/{id:int}")]
    public async Task<JsonResult> GetById(int id)
    {
        var teacher = await context.Teachers.FindAsync(id);
        return Json(teacher);
    }

    private Task<bool> TeacherExists(int id)
    {
        return context.Teachers.AnyAsync(e => e.Id == id);
    }
}
