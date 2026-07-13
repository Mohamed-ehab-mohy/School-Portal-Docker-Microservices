using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using students_mvc.Models;
using students_mvc.Services;

namespace students_mvc.Controllers;

[Authorize]
public class NotificationsController(INotificationService notificationService) : Controller
{
    public async Task<IActionResult> Index(int page = 1, string? search = null)
    {
        const int pageSize = 20;
        var userId = GetUserId();

        var notifications = await notificationService.GetAllNotificationsAsync(page, pageSize, search);
        var totalCount = await notificationService.GetTotalCountAsync(search);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.SearchTerm = search;
        ViewBag.TotalCount = totalCount;

        return View(notifications);
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = GetUserId();
        var notifications = await notificationService.GetUserNotificationsAsync(userId, 15);
        var unreadCount = await notificationService.GetUnreadCountAsync(userId);

        return Json(new
        {
            notifications = notifications.Select(n => new
            {
                n.Id,
                n.Title,
                n.Message,
                Type = n.Type.ToString().ToLower(),
                Category = n.Category.ToString().ToLower(),
                n.Link,
                n.IsRead,
                n.IconClass,
                TimeAgo = n.TimeAgo,
                CreatedAt = n.CreatedAt.ToString("MMM dd, yyyy HH:mm")
            }),
            unreadCount
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await notificationService.MarkAsReadAsync(id, GetUserId());
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await notificationService.MarkAllAsReadAsync(GetUserId());
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await notificationService.DeleteAsync(id, GetUserId());
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAll()
    {
        await notificationService.DeleteAllAsync(GetUserId());
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var count = await notificationService.GetUnreadCountAsync(GetUserId());
        return Json(new { count });
    }

    private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
}
