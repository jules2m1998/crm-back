using CRM.Core.Domain.Entities;

namespace CRM.Core.Business.Authentication;

public interface IJWTService
{
    string Generate(User user, List<Role> roles);
}
