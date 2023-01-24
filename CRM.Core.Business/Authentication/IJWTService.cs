using CRM.Core.Business.Models;

namespace CRM.Core.Business.Authentication;

public interface IJWTService
{
    string Generate(User user);
}
