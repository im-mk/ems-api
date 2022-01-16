using System;
using System.Threading;
using System.Threading.Tasks;
using EMS.Db;
using EMS.Domain.Db;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using EMS.Core.Errors;
using System.Net;

namespace EMS.Core.Holidays
{
    public class Create
    {
        public class Command : IRequest 
        {
            public string RequestedById { get; set; }
            public DateTime DateRequested { get; set; }
            public DateTime DateFrom { get; set; }
            public string DateFromPart { get; set; }
            public DateTime DateTo { get; set; }
            public string DateToPart { get; set; }
            public string Comments { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RequestedById).NotEmpty();
                RuleFor(x => x.DateRequested).NotEmpty();
                RuleFor(x => x.DateFrom).NotEmpty();
                RuleFor(x => x.DateFromPart).NotEmpty();
                RuleFor(x => x.DateTo).NotEmpty();
                RuleFor(x => x.DateToPart).NotEmpty();
                RuleFor(x => x.Comments).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;

            public Handler(
                DataContext context,
                UserManager<AppUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var requestedBy = await _userManager.FindByIdAsync(request.RequestedById);
                if (requestedBy == null)
                    throw new RestException(HttpStatusCode.Unauthorized);
                
                var holiday = new Holiday
                {
                    RequestedById = request.RequestedById,
                    DateRequested = request.DateRequested,
                    DateFrom = request.DateFrom,
                    DateFromPart = request.DateFromPart,
                    DateTo = request.DateTo,
                    DateToPart = request.DateToPart,
                    Comments = request.Comments,
                    Status = "Request Created",
                    StatusById = request.RequestedById,
                    StatusDate = request.DateRequested
                };

                _context.Holidays.Add(holiday);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}