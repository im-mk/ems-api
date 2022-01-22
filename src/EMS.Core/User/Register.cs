using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Errors;
using EMS.Core.Interfaces;
using EMS.Core.Validators;
using EMS.Db;
using EMS.Domain.Db;
using EMS.Core.Dto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EMS.Core.Mappers;

namespace EMS.Core.User
{
    public class Register
    {
        public class Command : IRequest<UserBasic>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, UserBasic>
        {
            private readonly DataContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserBasicMapper _mapper;

            public Handler(
                DataContext context,
                UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator,
                IUserBasicMapper mapper)
            {
                _userManager = userManager;
                _context = context;
                _jwtGenerator = jwtGenerator;
                _mapper = mapper;
            }

            public async Task<UserBasic> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Username already exists" });

                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return _mapper.Map(user, _jwtGenerator.CreateToken(user));
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}