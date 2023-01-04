using CRM.api.Models;

namespace CRM.api.Services;

public interface IJWTService
{
    string GenerateToken(User user);
}
