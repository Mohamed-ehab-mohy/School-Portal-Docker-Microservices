using Microsoft.EntityFrameworkCore;
using students_mvc.Data;
using students_mvc.Models;

namespace students_mvc.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> CreateAsync(string title, string message, NotificationType type, NotificationCategory category, string? link = null, string? userId = null, string? iconClass = null)
    {
        var notification = new Notification
        {
            Title = title,
            Message = message,
            Type = type,
            Category = category,
            Link = link,
            UserId = userId,
            IconClass = iconClass ?? GetDefaultIcon(category, type),
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string? userId, int limit = 20)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId || n.UserId == null)
            .OrderByDescending(n => n.CreatedAt);

        return await query.Take(limit).ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string? userId)
    {
        return await _context.Notifications
            .CountAsync(n => (n.UserId == userId || n.UserId == null) && !n.IsRead);
    }

    public async Task MarkAsReadAsync(int notificationId, string? userId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null && (notification.UserId == userId || notification.UserId == null))
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string? userId)
    {
        var unread = await _context.Notifications
            .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
            .ToListAsync();

        foreach (var n in unread)
        {
            n.IsRead = true;
            n.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int notificationId, string? userId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null && (notification.UserId == userId || notification.UserId == null))
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync(string? userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();

        _context.Notifications.RemoveRange(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetAllNotificationsAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _context.Notifications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(n =>
                n.Title.ToLower().Contains(searchLower) ||
                n.Message.ToLower().Contains(searchLower));
        }

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _context.Notifications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(n =>
                n.Title.ToLower().Contains(searchLower) ||
                n.Message.ToLower().Contains(searchLower));
        }

        return await query.CountAsync();
    }

    private static string GetDefaultIcon(NotificationCategory category, NotificationType type) => category switch
    {
        NotificationCategory.Student => "bi-person-plus",
        NotificationCategory.Teacher => "bi-person-badge",
        NotificationCategory.Attendance => "bi-clipboard-check",
        NotificationCategory.Grade => "bi-journal-text",
        NotificationCategory.Class => "bi-building",
        NotificationCategory.System => type switch
        {
            NotificationType.Success => "bi-check-circle-fill",
            NotificationType.Warning => "bi-exclamation-triangle-fill",
            NotificationType.Error => "bi-x-circle-fill",
            _ => "bi-info-circle-fill"
        },
        _ => "bi-bell"
    };
}
