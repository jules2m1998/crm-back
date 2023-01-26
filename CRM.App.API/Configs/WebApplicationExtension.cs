using CRM.App.API.Middlewares;
using System.Globalization;

namespace CRM.App.API.Configs;

public static class WebApplicationExtension
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    public static WebApplication ConfigureStaticFiles(this WebApplication app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                // Cache static files for 30 days
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
                ctx.Context.Response.Headers.Append("Expires",
                    DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
                ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            },
        });

        return app;
    }
}
