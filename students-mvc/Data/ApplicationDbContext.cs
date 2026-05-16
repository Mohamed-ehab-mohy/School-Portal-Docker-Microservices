using Microsoft.EntityFrameworkCore;
using students_mvc.Models;

namespace students_mvc.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
