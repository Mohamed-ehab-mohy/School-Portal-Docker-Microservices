using System.ComponentModel.DataAnnotations;

namespace students_mvc.Models;

public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

public enum NotificationCategory
{
    System,
    Student,
    Teacher,
    Attendance,
    Grade,
    Class
}

public class Notification
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public NotificationType Type { get; set; } = NotificationType.Info;

    [Required]
    public NotificationCategory Category { get; set; } = NotificationCategory.System;

    [StringLength(500)]
    public string? Link { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(100)]
    public string? IconClass { get; set; }

    public string TimeAgo
    {
        get
        {
            var span = DateTime.UtcNow - CreatedAt;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
            return CreatedAt.ToString("MMM dd, yyyy");
        }
    }
}
