using students_mvc.Models;

namespace students_mvc.Services;

public interface INotificationService
{
    Task<Notification> CreateAsync(string title, string message, NotificationType type, NotificationCategory category, string? link = null, string? userId = null, string? iconClass = null);
    Task<List<Notification>> GetUserNotificationsAsync(string? userId, int limit = 20);
    Task<int> GetUnreadCountAsync(string? userId);
    Task MarkAsReadAsync(int notificationId, string? userId);
    Task MarkAllAsReadAsync(string? userId);
    Task DeleteAsync(int notificationId, string? userId);
    Task DeleteAllAsync(string? userId);
    Task<List<Notification>> GetAllNotificationsAsync(int page = 1, int pageSize = 20, string? search = null);
    Task<int> GetTotalCountAsync(string? search = null);
}
