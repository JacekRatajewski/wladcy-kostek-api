using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WladcyKostek.Core.Requests.Commands;
using WladcyKostek.Core.Requests.Queries;

namespace WladcyKostek.Api.Controllers
{
    [ApiController]
    [Route("bonus")]
    public class BonusesController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;

        public BonusesController(ILogger<NewsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpGet("by/{name}")]
        public async Task<IActionResult> GetBonuses(string name)
        {
            var response = await _mediator.Send(new GetBonusIdQuery { Name = name });
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBonusId(string id)
        {
            var response = await _mediator.Send(new GetBonusesQuery { Id = int.Parse(id) });
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
