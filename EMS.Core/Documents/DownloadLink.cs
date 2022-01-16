using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.AWS;
using EMS.Core.Errors;
using EMS.Db;
using FluentValidation;
using MediatR;

namespace EMS.Core.Documents
{
    public class DownloadLink
    {
        public class Query : IRequest<string>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, string>
        {
            private readonly DataContext _context;
            private readonly IS3Service _s3Service;

            public Handler(DataContext context, IS3Service s3Service)
            {
                _s3Service = s3Service;
                _context = context;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var document = await _context.Documents.FindAsync(request.Id);

                if (document == null)
                    throw new RestException(HttpStatusCode.NotFound, new { document = "Not found" });

                return _s3Service.GenereatePresignedURL(document.Path);
            }
        }
    }
}