using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EMS.Core.Documents;

namespace EMS.Api.Controllers
{
    public class DocumentsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EMS.Domain.Db.Document>>> List([FromQuery]List.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromForm]Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("link/{id}")]
        public async Task<ActionResult<string>> CreateDownloadLink(Guid id)
        {
            return await Mediator.Send(new DownloadLink.Query{ Id = id});
        }
    }
}
