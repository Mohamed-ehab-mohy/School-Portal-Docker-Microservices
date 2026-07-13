using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using grades_mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Controllers;

[Authorize]
public class GradesController(
    GradesDbContext context,
    IStudentsServiceClient studentsClient) : Controller
{
    public async Task<IActionResult> Index()
    {
        var grades = await context.Grades.ToListAsync();
        var studentIds = grades.Select(g => g.StudentId).Distinct();
        var students = await studentsClient.GetStudentsByIdsAsync(studentIds);

        var viewModels = grades.Select(grade =>
        {
            students.TryGetValue(grade.StudentId, out var student);
            return ToViewModel(grade, student);
        }).ToList();

        return View(viewModels);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var grade = await context.Grades.FirstOrDefaultAsync(m => m.Id == id);
        if (grade is null)
        {
            return NotFound();
        }

        var student = await studentsClient.GetStudentByIdAsync(grade.StudentId);
        return View(ToViewModel(grade, student));
    }

    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create()
    {
        await PopulateStudentDropdownAsync();
        return View(new GradeFormViewModel { GradeDate = DateTime.UtcNow.Date });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(GradeFormViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var grade = new Grade
            {
                StudentId = viewModel.StudentId,
                CourseName = viewModel.CourseName,
                Score = viewModel.Score,
                GradeDate = viewModel.GradeDate,
                Notes = viewModel.Notes
            };

            context.Add(grade);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateStudentDropdownAsync();
        return View(viewModel);
    }

    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var grade = await context.Grades.FindAsync(id);
        if (grade is null)
        {
            return NotFound();
        }

        var student = await studentsClient.GetStudentByIdAsync(grade.StudentId);

        return View(new GradeFormViewModel
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = student?.FullName ?? $"Student #{grade.StudentId}",
            CourseName = grade.CourseName,
            Score = grade.Score,
            GradeDate = grade.GradeDate,
            Notes = grade.Notes
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Edit(int id, GradeFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var grade = await context.Grades.FindAsync(id);
                if (grade is null)
                {
                    return NotFound();
                }

                grade.CourseName = viewModel.CourseName;
                grade.Score = viewModel.Score;
                grade.GradeDate = viewModel.GradeDate;
                grade.Notes = viewModel.Notes;
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GradeExists(viewModel.Id))
                {
                    return NotFound();
                }

                throw;
            }
        }

        await PopulateStudentDropdownAsync();
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var grade = await context.Grades.FirstOrDefaultAsync(m => m.Id == id);
        if (grade is null)
        {
            return NotFound();
        }

        var student = await studentsClient.GetStudentByIdAsync(grade.StudentId);
        return View(ToViewModel(grade, student));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var grade = await context.Grades.FindAsync(id);
        if (grade is not null)
        {
            context.Grades.Remove(grade);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private static GradeViewModel ToViewModel(Grade grade, Student? student)
    {
        return new GradeViewModel
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = student?.FullName ?? $"Student #{grade.StudentId}",
            CourseName = grade.CourseName,
            Score = grade.Score,
            GradeDate = grade.GradeDate,
            Notes = grade.Notes
        };
    }

    private async Task PopulateStudentDropdownAsync()
    {
        var students = await studentsClient.GetAllStudentsAsync();
        ViewBag.StudentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            students, "Id", "FullName");
    }

    private Task<bool> GradeExists(int id)
    {
        return context.Grades.AnyAsync(e => e.Id == id);
    }
}
