using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Supervision;

public class SupervisionOutModel
{
    public UserModel Supervised { get; set; }
    public UserModel Supervisor { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public SupervisionOutModel(UserModel supervised, UserModel supervisor, bool isActive, DateTime createdAt)
    {
        Supervised = supervised;
        Supervisor = supervisor;
        IsActive = isActive;
        CreatedAt = createdAt;
    }
}
