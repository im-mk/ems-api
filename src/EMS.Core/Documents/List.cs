using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMS.Db;
using EMS.Domain.Db;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.Documents
{
    public class List
    {
        public class Query : IRequest<List<Document>> 
        {
            public int Page { get; set; }
            public int Size { get; set; }
            public int Skip 
            {
                get { return Size * (Page-1); }
            }
        }

        public class Handler : IRequestHandler<Query, List<Document>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Document>> Handle(Query request, CancellationToken cancellationToken)
            {
                var documents = await _context.Documents.Take(request.Size).Skip(request.Skip).ToListAsync();
                
                return documents;
            }
        }
    }
}