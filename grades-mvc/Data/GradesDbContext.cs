using grades_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Data;

public class GradesDbContext(DbContextOptions<GradesDbContext> options) : DbContext(options)
{
    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Grade>()
            .Property(grade => grade.Score)
            .HasPrecision(5, 2);
    }
}
