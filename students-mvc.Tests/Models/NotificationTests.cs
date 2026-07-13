using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class NotificationTests
{
    [Fact]
    public void Notification_DefaultProperties()
    {
        var notification = new Notification();
        Assert.Equal(string.Empty, notification.Title);
        Assert.Equal(string.Empty, notification.Message);
        Assert.Equal(NotificationType.Info, notification.Type);
        Assert.Equal(NotificationCategory.System, notification.Category);
        Assert.False(notification.IsRead);
        Assert.Null(notification.UserId);
        Assert.Null(notification.Link);
        Assert.Null(notification.ReadAt);
    }

    [Fact]
    public void Notification_CreatedAt_DefaultsToUtcNow()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var notification = new Notification();
        var after = DateTime.UtcNow.AddSeconds(1);
        Assert.InRange(notification.CreatedAt, before, after);
    }

    [Fact]
    public void Notification_TimeAgo_JustNow()
    {
        var notification = new Notification { CreatedAt = DateTime.UtcNow.AddSeconds(-30) };
        Assert.Equal("Just now", notification.TimeAgo);
    }

    [Fact]
    public void Notification_TimeAgo_MinutesAgo()
    {
        var notification = new Notification { CreatedAt = DateTime.UtcNow.AddMinutes(-5) };
        Assert.Equal("5m ago", notification.TimeAgo);
    }

    [Fact]
    public void Notification_TimeAgo_HoursAgo()
    {
        var notification = new Notification { CreatedAt = DateTime.UtcNow.AddHours(-3) };
        Assert.Equal("3h ago", notification.TimeAgo);
    }

    [Fact]
    public void Notification_TimeAgo_DaysAgo()
    {
        var notification = new Notification { CreatedAt = DateTime.UtcNow.AddDays(-2) };
        Assert.Equal("2d ago", notification.TimeAgo);
    }

    [Fact]
    public void Notification_TimeAgo_WeeksAgo_ShowsDate()
    {
        var notification = new Notification { CreatedAt = DateTime.UtcNow.AddDays(-10) };
        Assert.Contains("2026", notification.TimeAgo);
    }

    [Fact]
    public void Notification_Type_CanBeSet()
    {
        var notification = new Notification { Type = NotificationType.Warning };
        Assert.Equal(NotificationType.Warning, notification.Type);
    }

    [Fact]
    public void Notification_Category_CanBeSet()
    {
        var notification = new Notification { Category = NotificationCategory.Attendance };
        Assert.Equal(NotificationCategory.Attendance, notification.Category);
    }

    [Fact]
    public void Notification_IsRead_CanBeMarked()
    {
        var notification = new Notification { IsRead = false };
        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        Assert.True(notification.IsRead);
        Assert.NotNull(notification.ReadAt);
    }
}
