using System.Net;
using System.Text.Json;
using grades_mvc.Models;

namespace grades_mvc.Services;

public class StudentsServiceClient : IStudentsServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentsServiceClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public StudentsServiceClient(HttpClient httpClient, ILogger<StudentsServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/students/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Student>(content, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch student {StudentId} from students service", id);
            return null;
        }
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/students");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Student>>(content, JsonOptions) ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch students from students service");
            return [];
        }
    }

    public async Task<Dictionary<int, Student>> GetStudentsByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        var result = new Dictionary<int, Student>();

        foreach (var id in idList)
        {
            var student = await GetStudentByIdAsync(id);
            if (student is not null)
            {
                result[id] = student;
            }
        }

        return result;
    }
}
