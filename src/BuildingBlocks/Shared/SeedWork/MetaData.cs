namespace Shared.SeedWork;

public class MetaData
{
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public int PageSize { get; set; }
    public long TotalItems { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int FirstRowOnPage => TotalItems > 0 ? (CurrentPage - 1) * PageSize + 1 : 0;
    public int LastRowOnPage => (int)Math.Min(CurrentPage * PageSize, TotalItems);
}