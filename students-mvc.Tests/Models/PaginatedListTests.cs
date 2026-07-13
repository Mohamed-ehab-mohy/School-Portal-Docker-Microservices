using students_mvc.Models;

namespace students_mvc.Tests.Models;

public class PaginatedListTests
{
    [Fact]
    public void PaginatedList_CalculatesTotalPages_Correctly()
    {
        var items = new List<int> { 1, 2, 3 };
        var paginated = new PaginatedList<int>(items, 25, 1, 10, null);

        Assert.Equal(3, paginated.TotalPages);
        Assert.Equal(25, paginated.TotalCount);
    }

    [Fact]
    public void PaginatedList_RoundsUpTotalPages()
    {
        var items = new List<int> { 1, 2 };
        var paginated = new PaginatedList<int>(items, 11, 1, 10, null);

        Assert.Equal(2, paginated.TotalPages);
    }

    [Fact]
    public void PaginatedList_HasPreviousPage_IsFalseOnFirstPage()
    {
        var items = new List<int> { 1, 2 };
        var paginated = new PaginatedList<int>(items, 10, 1, 10, null);

        Assert.False(paginated.HasPreviousPage);
    }

    [Fact]
    public void PaginatedList_HasPreviousPage_IsTrueOnSecondPage()
    {
        var items = new List<int> { 1 };
        var paginated = new PaginatedList<int>(items, 20, 2, 10, null);

        Assert.True(paginated.HasPreviousPage);
    }

    [Fact]
    public void PaginatedList_HasNextPage_IsTrueWhenMorePagesExist()
    {
        var items = new List<int> { 1 };
        var paginated = new PaginatedList<int>(items, 20, 1, 10, null);

        Assert.True(paginated.HasNextPage);
    }

    [Fact]
    public void PaginatedList_HasNextPage_IsFalseOnLastPage()
    {
        var items = new List<int> { 1 };
        var paginated = new PaginatedList<int>(items, 10, 1, 10, null);

        Assert.False(paginated.HasNextPage);
    }

    [Fact]
    public void PaginatedList_SearchTerm_IsStored()
    {
        var items = new List<int>();
        var paginated = new PaginatedList<int>(items, 0, 1, 10, "test search");

        Assert.Equal("test search", paginated.SearchTerm);
    }

    [Fact]
    public void PaginatedList_SearchTerm_IsNullWhenNotProvided()
    {
        var items = new List<int>();
        var paginated = new PaginatedList<int>(items, 0, 1, 10, null);

        Assert.Null(paginated.SearchTerm);
    }

    [Fact]
    public void PaginatedList_EmptyList_ReturnsZeroTotalPages()
    {
        var items = new List<int>();
        var paginated = new PaginatedList<int>(items, 0, 1, 10, null);

        Assert.Equal(0, paginated.TotalPages);
        Assert.False(paginated.HasNextPage);
        Assert.False(paginated.HasPreviousPage);
    }

    [Fact]
    public void PaginatedList_ContainsAllItems()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var paginated = new PaginatedList<int>(items, 5, 1, 10, null);

        Assert.Equal(5, paginated.Count);
        Assert.Equal(5, paginated.Last());
    }
}
