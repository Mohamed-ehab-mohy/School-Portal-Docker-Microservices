using Microsoft.AspNetCore.Mvc;
using students_mvc.Models;

namespace students_mvc.ViewComponents;

public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PaginatedList<dynamic> items, string? searchTerm = null)
    {
        ViewBag.SearchTerm = searchTerm;
        return View(items);
    }
}
