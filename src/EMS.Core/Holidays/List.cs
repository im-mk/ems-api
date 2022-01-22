using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.Dto;
using EMS.Core.Mappers;
using EMS.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.Holidays
{
    public class List
    {
        public class Query : IRequest<List<Holiday>> 
        { 
            public int Page { get; set; }
            public int Size { get; set; }
            public int Skip 
            {
                get { return Size * (Page-1); }
            }
        }

        public class Handler : IRequestHandler<Query, List<Holiday>>
        {
            private readonly DataContext _context;
            private readonly IHolidayMapper _mapper;

            public Handler(DataContext context, IHolidayMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<Holiday>> Handle(Query request, CancellationToken cancellationToken)
            {
                var holidays = await _context.Holidays
                    .Include(h => h.RequestedBy)
                    .Include(h => h.StatusBy)
                    .Take(request.Size)
                    .Skip(request.Skip)
                    .ToListAsync();

                var result = holidays.Select(holiday => _mapper.Map(holiday));

                return result.ToList();
            }
        }
    }
}