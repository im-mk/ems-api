using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Errors;
using EMS.Db;
using MediatR;

namespace EMS.Core.Holidays
{
    public class Delete
    {
        public class Command : IRequest 
        {
            public int Id { get; set; }
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

                _context.Holidays.Remove(holiday);

                var success = await _context.SaveChangesAsync() > 0;
                
                if (success) 
                    return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}