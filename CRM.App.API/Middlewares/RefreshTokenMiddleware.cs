namespace CRM.App.API.Middlewares
{
    public class RefreshTokenMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await _next(context);
        }
    }
}
