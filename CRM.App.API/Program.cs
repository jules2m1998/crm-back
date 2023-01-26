using CRM.App.API.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add security, mediar, all dependencies and compression
builder.Services
    .AddSecurity(builder.Configuration)
    .AddMediaRConfig()
    .AddDependencies()
    .AddCompression();


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

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();
