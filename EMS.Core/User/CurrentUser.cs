using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Domain.Db;
using EMS.Core.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using EMS.Core.Mappers;

namespace EMS.Core.User
{
    public class CurrentUser
    {
        public class Query : IRequest<UserBasic> { }
        public class Handler : IRequestHandler<Query, UserBasic>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly IUserBasicMapper _mapper;

            public Handler(
                UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator,
                IUserAccessor userAccessor,
                IUserBasicMapper mapper)
            {
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<UserBasic> Handle(
                Query request, 
                CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return _mapper.Map(user, _jwtGenerator.CreateToken(user));
            }
        }
    }
}