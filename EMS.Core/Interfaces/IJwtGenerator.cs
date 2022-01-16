using EMS.Domain.Db;

namespace EMS.Core.Interfaces
{
    public interface IJwtGenerator
    {
         string CreateToken(AppUser user);
    }
}