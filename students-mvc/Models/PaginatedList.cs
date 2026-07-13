using Microsoft.EntityFrameworkCore;

namespace students_mvc.Models;

public interface IPaginatedList
{
    int PageIndex { get; }
    int TotalPages { get; }
    int TotalCount { get; }
    string? SearchTerm { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}

public class PaginatedList<T> : List<T>, IPaginatedList
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }
    public string? SearchTerm { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, string? searchTerm)
    {
        PageIndex = pageIndex;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        SearchTerm = searchTerm;
        AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize, string? searchTerm = null)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize, searchTerm);
    }
}
