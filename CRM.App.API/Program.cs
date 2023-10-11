using CRM.App.API.Configs;
using CRM.Core.Business.Settings;
using CRM.Core.Business;
using CRM.Core.Business.Mappers;
using CRM.Infra.Data;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add security, mediar, all dependencies and compression
builder.Services
    .AddSecurity(builder.Configuration)
    .AddCompression();

builder.Services
    .AddInfrastructureServices()
    .AddBusinessServices();

//builder.Services.AddHostedService<SendEmailService>();

builder.Services.AddCors();

builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SmtpSettings")); // crm.globalasset@gmail.com // +e$7^UZ-X2Nd_n=5


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Add all middlewares
app.AddMiddlewares()
    .ConfigureStaticFiles();
app.UseCors(opt => opt
    .WithOrigins("http://localhost:3000", "http://localhost:8000", "http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

await app.ManageApp();

app.Run();
