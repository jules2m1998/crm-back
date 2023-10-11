using CRM.Core.Business.Authentication;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Infra.Data.Helpers;
using CRM.Infra.Data.Repositories;
using CRM.Infra.Data.Services;
using CRM.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infra.Data;

public static class InfraServiceRegistration // ServiceCollectionRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection @this)
    {
        @this.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));


        @this.AddScoped<IJWTService, JWTService>();
        @this.AddScoped<IUserRepository, UserRepository>();
        @this.AddScoped<IFileHelper, FileHelper>();
        @this.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
        @this.AddScoped<ISkillRepository, SkillRepository>();
        @this.AddScoped<IProductRepository, ProductRepository>();
        @this.AddScoped<ICompanyRepository, CompanyRepository>();
        @this.AddScoped<ISupervisionHistoryRepository, SupervisionHistoryRepository>();
        @this.AddScoped<IProspectionRepository, ProspectionRepository>();
        @this.AddScoped<IContactRepository, ContactRepository>();
        @this.AddScoped<IPhoneRepository, PhoneRepository>();
        @this.AddScoped<IEventRepository, EventRepository>();
        @this.AddScoped<IEmailService, EmailService>();
        @this.AddScoped<IEmailRepository, EmailRepository>();
        @this.AddScoped<IProductStageRepository, ProductStageRepository>();
        @this.AddScoped<IStageResponseRepository, StageResponseRepository>();
        @this.AddScoped<ICommitRepository, CommitRepository>();
        @this.AddScoped<IHeadProspectionRepository, HeadProspectionRepository>();
        @this.AddScoped<IResponseRepository, ResponseRepository>();
        @this.AddScoped<IStageRepository, StageRepository>();

        return @this;
    }
}
