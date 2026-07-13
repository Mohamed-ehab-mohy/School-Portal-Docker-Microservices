using grades_mvc.Models;

namespace grades_mvc.Services;

public interface IStudentsServiceClient
{
    Task<Student?> GetStudentByIdAsync(int id);
    Task<List<Student>> GetAllStudentsAsync();
    Task<Dictionary<int, Student>> GetStudentsByIdsAsync(IEnumerable<int> ids);
}
