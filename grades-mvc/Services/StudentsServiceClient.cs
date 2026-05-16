using grades_mvc.Models;

namespace grades_mvc.Services;

public class StudentsServiceClient(HttpClient httpClient)
{
    public async Task<List<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<StudentDto>>("api/students", cancellationToken)
            ?? [];
    }

    public async Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<StudentDto>($"api/students/{id}", cancellationToken);
    }
}
