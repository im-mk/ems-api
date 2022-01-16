using EMS.Core.Dto;
using EMS.Domain.Db;

namespace EMS.Core.Mappers
{
    public interface IUserBasicMapper
    {
        UserBasic Map(AppUser user, string token);
    }
}