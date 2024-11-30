using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WladcyKostek.Core.Requests.Commands;
using WladcyKostek.Core.Requests.Queries;

namespace WladcyKostek.Api.Controllers
{
    [ApiController]
    [Route("bonuses")]
    public class BonusesController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;

        public BonusesController(ILogger<NewsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBonuses([FromBody] GetBonusesQuery command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBonusId([FromBody] GetBonusIdQuery command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
