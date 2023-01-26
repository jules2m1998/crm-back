namespace CRM.App.API
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
