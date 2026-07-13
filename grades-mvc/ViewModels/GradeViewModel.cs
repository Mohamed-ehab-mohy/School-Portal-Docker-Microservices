namespace grades_mvc.ViewModels;

public class GradeViewModel
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public decimal Score { get; set; }

    public DateTime GradeDate { get; set; }

    public string? Notes { get; set; }
}
