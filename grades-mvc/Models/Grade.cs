using System.ComponentModel.DataAnnotations;

namespace grades_mvc.Models;

public class Grade
{
    public int Id { get; set; }

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
