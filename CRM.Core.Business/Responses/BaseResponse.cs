namespace CRM.Core.Business.Responses;

public class BaseResponse<T> where T : class
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; } = null!;
    public List<string>? Errors { get; set; }
}
