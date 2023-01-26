using CRM.App.API.Configs;
using CRM.App.API.Middlewares;
using CRM.Core.Business.Authentication;
using CRM.Core.Business.Repositories;
using CRM.Infra.Data;
using CRM.Infra.Data.Repositories;
using CRM.Infra.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddSecurity(builder.Configuration)
    .AddMediaRConfig()
    .AddDependencies();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();
app.Run();
