using System;
using System.Threading;
using System.Threading.Tasks;
using EMS.Db;
using EMS.Domain.Db;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using EMS.Core.AWS;

namespace EMS.Core.Documents
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Comment { get; set; }
            public IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Comment).NotEmpty();
                RuleFor(x => x.File).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IS3Service _s3Service;

            public Handler(DataContext context, IS3Service s3Service)
            {
                _s3Service = s3Service;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var id = Guid.NewGuid();
                var fileExtension = System.IO.Path.GetExtension(request.File.FileName);
                var path = string.Format("documents/{0}{1}", id.ToString(), fileExtension);

                await _s3Service.UploadFileToS3(request.File, path);

                var document = new Document
                {
                    DocumentId = id,
                    Title = request.Title,
                    Comments = request.Comment,
                    Path = path,
                    DateUploaded = DateTime.Now
                };

                _context.Documents.Add(document);

                var success = await _context.SaveChangesAsync() > 0;
                
                if (success) 
                    return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}