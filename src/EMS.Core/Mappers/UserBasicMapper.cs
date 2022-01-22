using EMS.Core.Dto;
using EMS.Domain.Db;

namespace EMS.Core.Mappers
{
    public class UserBasicMapper : IUserBasicMapper
    {
        public UserBasic Map(AppUser user, string token)
        {
            return new UserBasic
            {
                DisplayName = user.DisplayName,
                Token = token,
                Username = user.UserName,
                Image = null
            };
        }
    }
}