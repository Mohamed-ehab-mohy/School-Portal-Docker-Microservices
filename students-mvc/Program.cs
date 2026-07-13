using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using students_mvc.Data;
using students_mvc.Messaging;
using students_mvc.Models;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(Environment.GetEnvironmentVariable("Serilog__SeqServer") ?? "http://seq:5341")
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "students-mvc")
    .CreateLogger();

try
{
    Log.Information("Starting students-mvc");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IStudentMessageBus, RabbitMqStudentMessageBus>();
builder.Services.AddScoped<students_mvc.Services.INotificationService, students_mvc.Services.NotificationService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "SchoolPortal.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "postgresql");

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Admin", "Teacher", "Student", "Parent" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminPassword = builder.Configuration["AdminPassword"] ?? "Admin@123";
    const string adminEmail = "admin@school.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    if (!context.Notifications.Any())
    {
        context.Notifications.AddRange(
            new Notification
            {
                Title = "Welcome to School Portal",
                Message = "The notification system is now active. You will receive real-time updates for student, teacher, and attendance events.",
                Type = NotificationType.Info,
                Category = NotificationCategory.System,
                IconClass = "bi-bell-fill"
            },
            new Notification
            {
                Title = "New School Year Started",
                Message = "The 2025-2026 academic year has officially begun. All classes are now active.",
                Type = NotificationType.Success,
                Category = NotificationCategory.System,
                IconClass = "bi-calendar-check"
            },
            new Notification
            {
                Title = "System Update",
                Message = "Dashboard analytics and report generation features are now available for administrators.",
                Type = NotificationType.Info,
                Category = NotificationCategory.System,
                IconClass = "bi-gear-fill"
            },
            new Notification
            {
                Title = "Attendance Tracking Active",
                Message = "Daily attendance tracking is now enabled. Teachers can submit attendance from the Attendance page.",
                Type = NotificationType.Info,
                Category = NotificationCategory.Attendance,
                IconClass = "bi-clipboard-check"
            },
            new Notification
            {
                Title = "Grades Management Online",
                Message = "The grades management system is operational. Teachers can now enter and manage student grades.",
                Type = NotificationType.Success,
                Category = NotificationCategory.Grade,
                IconClass = "bi-journal-text"
            }
        );
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
