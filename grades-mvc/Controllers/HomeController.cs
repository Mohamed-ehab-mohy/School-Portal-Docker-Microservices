using System.Diagnostics;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Controllers;

public class HomeController(
    GradesDbContext context,
    StudentsServiceClient studentsServiceClient,
    ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.GradeCount = await context.Grades.CountAsync();
        ViewBag.SubjectCount = await context.Grades.Select(g => g.Subject).Distinct().CountAsync();

        try
        {
            var students = await studentsServiceClient.GetAllAsync();
            ViewBag.StudentsServiceOnline = true;
            ViewBag.ConnectedStudentCount = students.Count;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Students service unavailable on home page.");
            ViewBag.StudentsServiceOnline = false;
            ViewBag.ConnectedStudentCount = 0;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Students service timed out on home page.");
            ViewBag.StudentsServiceOnline = false;
            ViewBag.ConnectedStudentCount = 0;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
