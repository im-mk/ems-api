using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Dto;
using EMS.Core.Errors;
using EMS.Core.Mappers;
using EMS.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.Holidays
{
    public class Details
    {
        public class Query : IRequest<Holiday>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Holiday>
        {
            private readonly DataContext _context;
            private readonly IHolidayMapper _mapper;
            
            public Handler(DataContext context, IHolidayMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Holiday> Handle(Query request, CancellationToken cancellationToken)
            {
                var holiday = await _context.Holidays
                .Include(h => h.RequestedBy)
                .Include(h => h.StatusBy)
                .SingleOrDefaultAsync(h => h.HolidayId == request.Id);

                if (holiday == null)
                    throw new RestException(HttpStatusCode.NotFound, new { holiday = "Not found" });

                return _mapper.Map(holiday);
            }
        }
    }
}