using System.ComponentModel.DataAnnotations;

namespace grades_mvc.ViewModels;

public class GradeFormViewModel
{
    public int Id { get; set; }

    public string StudentName { get; set; } = string.Empty;

    [Required]
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime GradeDate { get; set; }
}
