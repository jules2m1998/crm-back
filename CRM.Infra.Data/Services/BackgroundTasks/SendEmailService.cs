using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Services.BackgroundTasks;

public class SendEmailService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<SendEmailService> _logger;

    public SendEmailService(IServiceScopeFactory factory, ILogger<SendEmailService> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start send email.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _factory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
            ICollection<Email> mails = await repo.GetCurrentsAsync();
            if (mails.Any()) await ParalleliseSendAction(mails);
            else await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task ParalleliseSendAction(ICollection<Email> mails)
    {
        string now = DateTime.UtcNow.ToString("F");
        int count = mails.Count;

        _logger.LogInformation("{count} Send at {now}", count, now);
        using var scope = _factory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
        var service = scope.ServiceProvider.GetRequiredService<IEmailService>();
        foreach (var mail in mails)
        {
            switch (mail.EmailType)
            {
                case EmailType.SECOND:
                    await service.SendSecondAsync(mail.Event);
                    break;
                case EmailType.LAST:
                    await service.SendLastAsync(mail.Event);
                    break;
                default:
                    await service.SendAsync(mail.Event);
                    break;
            }
            mail.IsSend = true;
            await repo.UpdateAsync(mail);
        }
    }
}
