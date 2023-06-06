using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Extensions;

public static class EntityToString
{
    public static string ToEmailString(this Contact c, Event e) => 
        ToMail(
            c.Name, 
            "Meeting Reminder", 
            $"This is a friendly reminder to inform you that the meeting for {e.Topic} is scheduled on {e.StartDate:D} at {e.StartDate:t}. We would greatly appreciate your presence.",
            $"{e.Owner.FirstName} {e.Owner.LastName}"
            );
    public static string ToEmailSecondString(this Contact c, Event e) => 
        ToMail(
            c.Name, 
            "Meeting Reminder", 
            $"Dear {c.Name}, we would like to inform you that your meeting is about to start in 5 minutes. Please prepare yourself and join the meeting room on time. Thank you!", 
            $"{e.Owner.FirstName} {e.Owner.LastName}"
            );
    public static string ToEmailLastString(this Contact c, Event e) => 
        ToMail(
            c.Name, 
            "Meeting Reminder", 
            $"Dear {c.Name}, we want to remind you that your meeting has just started. Please join the meeting room immediately. Thank you!", 
            $"{e.Owner.FirstName} {e.Owner.LastName}"
            );
    private static string ToMail(string receiverName, string title, string body, string senderFullName) => $@"
            <!DOCTYPE html>
            <html>
            <head>
              <style>
                /* CSS pour le corps de l'email */
                body {{
                  font-family: Arial, sans-serif;
                  background-color: #f9f9f9;
                  margin: 0;
                  padding: 0;
                }}
    
                /* CSS pour le conteneur principal */
                .container {{
                  max-width: 600px;
                  margin: 0 auto;
                  padding: 20px;
                  background-color: #ffffff;
                  border-radius: 10px;
                  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
                }}
    
                /* CSS pour le titre */
                h1 {{
                  color: #333333;
                  margin-top: 0;
                }}
    
                /* CSS pour le paragraphe */
                p {{
                  margin-bottom: 20px;
                  line-height: 1.5;
                }}
    
                /* CSS pour le bouton */
                .button {{
                  display: inline-block;
                  background-color: #4CAF50;
                  color: #ffffff;
                  text-decoration: none;
                  padding: 12px 24px;
                  border-radius: 30px;
                  font-weight: bold;
                  transition: background-color 0.3s ease;
                }}
    
                .button:hover {{
                  background-color: #45a049;
                }}
    
                /* CSS pour le pied de page */
                footer {{
                  text-align: center;
                  margin-top: 20px;
                  color: #888888;
                }}
              </style>
            </head>
            <body>
              <div class=""container"">
                <h1>{title}</h1>
    
                <p>Dear {receiverName},</p>
    
                <p>{body}</p>
    
                <p>{senderFullName}</p>
    
                <footer>
                  <p>This email was sent automatically. Please do not reply to it.</p>
                </footer>
              </div>
            </body>
            </html>


        ";
}
