using CRM.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CRM.App.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (BaseException e)
            {
                await HandleExceptionAsync(context, e);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, BaseException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var badRequest = new BadRequestObjectResult(exception.Errors);
            var errors = JsonSerializer.Serialize(badRequest);
            await context.Response.WriteAsync(errors);
        }
    }
}
