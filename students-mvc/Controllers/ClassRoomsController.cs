using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Controllers;

[Authorize]
public class ClassRoomsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var classRooms = await context.ClassRooms
            .Include(c => c.ClassTeachers)
                .ThenInclude(ct => ct.Teacher)
            .Include(c => c.StudentClasses)
            .ToListAsync();

        return View(classRooms);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var classRoom = await context.ClassRooms
            .Include(c => c.ClassTeachers)
                .ThenInclude(ct => ct.Teacher)
            .Include(c => c.StudentClasses)
                .ThenInclude(sc => sc.Student)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (classRoom is null)
        {
            return NotFound();
        }

        return View(classRoom);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await PopulateTeacherDropdownAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ClassRoom classRoom, int[] teacherIds)
    {
        if (ModelState.IsValid)
        {
            context.Add(classRoom);
            await context.SaveChangesAsync();

            if (teacherIds is { Length: > 0 })
            {
                foreach (var teacherId in teacherIds)
                {
                    context.ClassTeachers.Add(new ClassTeacher
                    {
                        ClassRoomId = classRoom.Id,
                        TeacherId = teacherId
                    });
                }
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        await PopulateTeacherDropdownAsync();
        return View(classRoom);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var classRoom = await context.ClassRooms
            .Include(c => c.ClassTeachers)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classRoom is null)
        {
            return NotFound();
        }

        ViewBag.SelectedTeacherIds = classRoom.ClassTeachers.Select(ct => ct.TeacherId).ToArray();
        await PopulateTeacherDropdownAsync();
        return View(classRoom);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, ClassRoom classRoom, int[] teacherIds)
    {
        if (id != classRoom.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(classRoom);
                await context.SaveChangesAsync();

                var existingAssignments = await context.ClassTeachers
                    .Where(ct => ct.ClassRoomId == id)
                    .ToListAsync();

                context.ClassTeachers.RemoveRange(existingAssignments);

                if (teacherIds is { Length: > 0 })
                {
                    foreach (var teacherId in teacherIds)
                    {
                        context.ClassTeachers.Add(new ClassTeacher
                        {
                            ClassRoomId = id,
                            TeacherId = teacherId
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClassRoomExists(classRoom.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await PopulateTeacherDropdownAsync();
        return View(classRoom);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var classRoom = await context.ClassRooms
            .FirstOrDefaultAsync(m => m.Id == id);

        if (classRoom is null)
        {
            return NotFound();
        }

        return View(classRoom);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var classRoom = await context.ClassRooms.FindAsync(id);
        if (classRoom is not null)
        {
            context.ClassRooms.Remove(classRoom);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/classes")]
    public async Task<JsonResult> GetAll()
    {
        var classes = await context.ClassRooms.ToListAsync();
        return Json(classes);
    }

    private async Task PopulateTeacherDropdownAsync()
    {
        var teachers = await context.Teachers.ToListAsync();
        ViewBag.TeacherList = new SelectList(teachers, "Id", "FullName");
    }

    private Task<bool> ClassRoomExists(int id)
    {
        return context.ClassRooms.AnyAsync(e => e.Id == id);
    }
}
