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

        // The important part -- configuring the SMTP client
        using SmtpClient smtp = new()
        {
            Port = _settings.Port,   // [1] You can try with 465 also, I always used 587 and got success
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network, // [2] Added this
            UseDefaultCredentials = false, // [3] Changed this
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),  // [4] Added this. Note, first parameter is NOT string.
            Host = _settings.Server
        };

        //recipient address
        mail.To.Add(new MailAddress(receiverEmail));

        //Formatted mail body
        mail.IsBodyHtml = true;

        var b = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <title>Meeting Invitation</title>\r\n  <style>\r\n    body {\r\n      font-family: Arial, sans-serif;\r\n      margin: 0;\r\n      padding: 20px;\r\n    }\r\n    h2 {\r\n      color: #333;\r\n    }\r\n    p {\r\n      margin-bottom: 10px;\r\n    }\r\n    ul {\r\n      list-style: none;\r\n      padding-left: 20px;\r\n    }\r\n    li:before {\r\n      content: \"•\";\r\n      color: #333;\r\n      font-weight: bold;\r\n      display: inline-block;\r\n      width: 1em;\r\n      margin-left: -1em;\r\n    }\r\n  </style>\r\n</head>\r\n<body>\r\n  <h2>Invitation à la réunion</h2>\r\n  <p>Bonjour,</p>\r\n  <p>Nous aimerions vous inviter à une réunion importante.</p>\r\n  \r\n  <h3>Détails de la réunion :</h3>\r\n  <ul>\r\n    <li>Date : [Insérer la date ici]</li>\r\n    <li>Heure : [Insérer l'heure ici]</li>\r\n    <li>Lieu : [Insérer le lieu ici]</li>\r\n    <li>Sujet : [Insérer le sujet ici]</li>\r\n  </ul>\r\n\r\n  <p>Merci de confirmer votre présence en répondant à cet e-mail.</p>\r\n  \r\n  <p>Cordialement,</p>\r\n  <p>[Votre nom]</p>\r\n</body>\r\n</html>\r\n";

        mail.Body = b;
        mail.Subject = subject;
        await smtp.SendMailAsync(mail);
    }
}
