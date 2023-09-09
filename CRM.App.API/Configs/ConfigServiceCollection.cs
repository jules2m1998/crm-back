using CRM.Core.Business;
using CRM.Core.Business.Authentication;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Domain.Entities;
using CRM.Infra.Data;
using CRM.Infra.Data.Helpers;
using CRM.Infra.Data.Repositories;
using CRM.Infra.Data.Services;
using CRM.Infra.Data.Services.BackgroundTasks;
using CRM.Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace CRM.App.API.Configs;

public static class ConfigureServiceCollection
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM api", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Add identity
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;
            // Default User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
            // Reset Sign in settings
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        // Database confifuration
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CRM.App.API")));


        // Auth with JWT configuration
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });


        return services;
    }
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileHelper, FileHelper>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ISupervisionHistoryRepository, SupervisionHistoryRepository>();
        services.AddScoped<IProspectionRepository, ProspectionRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IProductStageRepository, ProductStageRepository>();
        services.AddScoped<IStageResponseRepository, StageResponseRepository>();
        services.AddScoped<ICommitRepository, CommitRepository>();
        services.AddScoped<IHeadProspectionRepository, HeadProspectionRepository>();


        return services;
    }
    public static IServiceCollection AddMediaRConfig(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.AsScoped();
        }, Assembly.GetAssembly(typeof(MediatREntryPoint))!);
        return services;
    }
    public static IServiceCollection AddCompression(this IServiceCollection services)
    {
        // compression algorithm config
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        return services;
    }
}
