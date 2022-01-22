using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using EMS.Core.Errors;
using System.Net;
using FluentValidation;
using EMS.Core.Dto;
using EMS.Domain.Db;
using EMS.Core.Interfaces;
using EMS.Core.Mappers;

namespace EMS.Core.User
{
    public class Login
    {
        public class Query : IRequest<UserBasic>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, UserBasic>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserBasicMapper _mapper;

            public Handler(
                UserManager<AppUser> userManager, 
                SignInManager<AppUser> signInManager,
                IJwtGenerator jwtGenerator,
                IUserBasicMapper mapper)
            {
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<UserBasic> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {                    
                    return _mapper.Map(user, _jwtGenerator.CreateToken(user));
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}