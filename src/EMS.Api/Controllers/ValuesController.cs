using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly DataContext _context;

        public ValuesController(
            ILogger<ValuesController> logger,
            DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<EMS.Domain.Db.Value>>> Get()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
        }
    }
}