namespace Casa106.Application.DTOs;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => TotalItems == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
}
