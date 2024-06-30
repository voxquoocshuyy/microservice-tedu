namespace Shared.SeedWork;

public class ApiResult<T>
{
    public ApiResult(){}
    public ApiResult(bool isSucessed, string message = null)
    {
        Message = message;
        IsSucessed = isSucessed;
    }
    public ApiResult(bool isSucessed, T data, string message = null)
    {
        Data = data;
        Message = message;
        IsSucessed = isSucessed;
    }
    public bool IsSucessed { get; }
    public string Message { get; }
    public T Data { get; }
}