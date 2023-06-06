using CRM.Core.Business.Extensions;
using CRM.Core.Business.Services;
using CRM.Core.Business.Settings;
using CRM.Core.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSetting _settings;

    public EmailService(IOptions<SmtpSetting> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string subject, string body, string receiverEmail)
    {
        MailMessage mail = new()
        {
            From = new MailAddress(_settings.SenderEmail)
        };

        using SmtpClient smtp = new()
        {
            Port = _settings.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
            Host = _settings.Server
        };

        mail.To.Add(new MailAddress(receiverEmail));

        mail.IsBodyHtml = true;

        mail.Body = body;
        mail.Subject = subject;
        await smtp.SendMailAsync(mail);
    }

    public async Task SendAsync(Event e)
    {
        if (e.Contact == null) return;

        foreach(var contact in e.Contact)
        {
            var emailHtml = contact.ToEmailString(e);
            await SendAsync($"Appointment relative to : {e.Topic}", emailHtml, contact.Email);
        }
    }

    public async Task SendLastAsync(Event e)
    {
        if (e.Contact == null) return;

        foreach (var contact in e.Contact)
        {
            var emailHtml = contact.ToEmailLastString(e);
            await SendAsync($"Appointment reminder relative to : {e.Topic}", emailHtml, contact.Email);
        }
    }

    public async Task SendSecondAsync(Event e)
    {
        if (e.Contact == null) return;

        foreach (var contact in e.Contact)
        {
            var emailHtml = contact.ToEmailSecondString(e);
            await SendAsync($"Appointment reminder relative to : {e.Topic}", emailHtml, contact.Email);
        }
    }
}
