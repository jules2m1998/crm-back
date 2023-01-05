using CRM.core.Models;

namespace CRM.core.Services;

public interface IJWTService
{
    string GenerateToken(User user);
}
