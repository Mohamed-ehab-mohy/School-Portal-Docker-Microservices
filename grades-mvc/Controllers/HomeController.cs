using System.Diagnostics;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Controllers;

public class HomeController(GradesDbContext context, IStudentsServiceClient studentsClient) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.GradeCount = await context.Grades.CountAsync();
        ViewBag.SubjectCount = await context.Grades.Select(g => g.CourseName).Distinct().CountAsync();

        var students = await studentsClient.GetAllStudentsAsync();
        ViewBag.StudentCount = students.Count;

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
