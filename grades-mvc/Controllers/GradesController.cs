using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using grades_mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Controllers;

public class GradesController(
    GradesDbContext context,
    StudentsServiceClient studentsServiceClient,
    ILogger<GradesController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var grades = await context.Grades.ToListAsync();
        var studentsById = await GetStudentsByIdAsync();
        var isStudentServiceUnavailable = studentsById is null;

        var viewModels = grades.Select(grade => ToViewModel(grade, studentsById, isStudentServiceUnavailable));

        if (isStudentServiceUnavailable)
        {
            ViewData["WarningMessage"] = "Students service is unavailable. Showing student IDs instead.";
        }

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

        var student = await GetStudentByIdAsync(grade.StudentId);
        var isStudentServiceUnavailable = student is null;

        if (isStudentServiceUnavailable)
        {
            ViewData["WarningMessage"] = "Students service is unavailable. Showing the student ID instead.";
        }

        return View(ToViewModel(grade, student, isStudentServiceUnavailable));
    }

    public async Task<IActionResult> Create()
    {
        await PopulateStudentDropdownAsync();
        return View(new GradeFormViewModel { Date = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GradeFormViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var grade = new Grade
            {
                StudentId = viewModel.StudentId,
                Subject = viewModel.Subject,
                Score = viewModel.Score,
                Date = viewModel.Date
            };

            context.Add(grade);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateStudentDropdownAsync();
        return View(viewModel);
    }

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

        var student = await GetStudentByIdAsync(grade.StudentId);

        return View(new GradeFormViewModel
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = student?.FullName ?? $"Student #{grade.StudentId}",
            Subject = grade.Subject,
            Score = grade.Score,
            Date = grade.Date
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

                grade.Subject = viewModel.Subject;
                grade.Score = viewModel.Score;
                grade.Date = viewModel.Date;
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

        var student = await GetStudentByIdAsync(grade.StudentId);
        var isStudentServiceUnavailable = student is null;

        if (isStudentServiceUnavailable)
        {
            ViewData["WarningMessage"] = "Students service is unavailable. Showing the student ID instead.";
        }

        return View(ToViewModel(grade, student, isStudentServiceUnavailable));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
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

    private async Task<Dictionary<int, StudentDto>?> GetStudentsByIdAsync()
    {
        try
        {
            var students = await studentsServiceClient.GetAllAsync();
            return students.ToDictionary(student => student.Id);
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Students service request failed while loading all students.");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Students service request timed out while loading all students.");
            return null;
        }
    }

    private async Task<StudentDto?> GetStudentByIdAsync(int studentId)
    {
        try
        {
            return await studentsServiceClient.GetByIdAsync(studentId);
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Students service request failed while loading student {StudentId}.", studentId);
            return null;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Students service request timed out while loading student {StudentId}.", studentId);
            return null;
        }
    }

    private static GradeViewModel ToViewModel(
        Grade grade,
        Dictionary<int, StudentDto>? studentsById,
        bool isStudentServiceUnavailable)
    {
        StudentDto? student = null;
        studentsById?.TryGetValue(grade.StudentId, out student);

        return ToViewModel(grade, student, isStudentServiceUnavailable);
    }

    private static GradeViewModel ToViewModel(
        Grade grade,
        StudentDto? student,
        bool isStudentServiceUnavailable)
    {
        return new GradeViewModel
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = student?.FullName ?? $"Student #{grade.StudentId}",
            Subject = grade.Subject,
            Score = grade.Score,
            IsStudentServiceUnavailable = isStudentServiceUnavailable
        };
    }

    private async Task PopulateStudentDropdownAsync()
    {
        var students = await GetStudentsByIdAsync();
        if (students is not null)
        {
            ViewBag.StudentList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                students.Values, "Id", "FullName");
        }
    }

    private Task<bool> GradeExists(int id)
    {
        return context.Grades.AnyAsync(e => e.Id == id);
    }
}
