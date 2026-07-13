using System.Net;
using System.Text.Json;
using grades_mvc.Data;
using grades_mvc.Models;
using grades_mvc.Services;
using Microsoft.EntityFrameworkCore;

namespace grades_mvc.Services;

public class StudentsServiceClient : IStudentsServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentsServiceClient> _logger;
    private readonly IServiceProvider _serviceProvider;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public StudentsServiceClient(HttpClient httpClient, ILogger<StudentsServiceClient> logger, IServiceProvider serviceProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceProvider = serviceProvider;
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
            _logger.LogWarning(ex, "Failed to fetch student {StudentId} from students service, falling back to cache", id);
            return await GetStudentFromCacheAsync(id);
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
            _logger.LogWarning(ex, "Failed to fetch students from students service, falling back to cache");
            return await GetAllStudentsFromCacheAsync();
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

    private async Task<Student?> GetStudentFromCacheAsync(int id)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GradesDbContext>();
            var cached = await context.StudentCache.FindAsync(id);
            if (cached is null) return null;

            return new Student
            {
                Id = cached.Id,
                FirstName = cached.FirstName,
                LastName = cached.LastName,
                Email = cached.Email
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch student {StudentId} from cache", id);
            return null;
        }
    }

    private async Task<List<Student>> GetAllStudentsFromCacheAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GradesDbContext>();
            var cached = await context.StudentCache.ToListAsync();
            return cached.Select(c => new Student
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch students from cache");
            return [];
        }
    }
}
