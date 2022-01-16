using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Errors;
using EMS.Db;
using FluentValidation;
using MediatR;

namespace EMS.Core.Holidays
{
    public class Edit
    {
        public class Command : IRequest 
        {
            public int Id { get; set; }
            public string Comments { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Comments).NotEmpty();             
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var holiday = await _context.Holidays.FindAsync(request.Id);

                if (holiday == null)
                    throw new RestException(HttpStatusCode.NotFound, new { holiday = "Not found" });

                holiday.Comments = request.Comments;
                
                var success = await _context.SaveChangesAsync() > 0;
                
                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}