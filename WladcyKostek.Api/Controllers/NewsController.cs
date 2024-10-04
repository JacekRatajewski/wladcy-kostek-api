using MediatR;
using Microsoft.AspNetCore.Mvc;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Api.Controllers
{
    [ApiController]
    [Route("news")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;

        public NewsController(ILogger<NewsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PostNews([FromBody] AddNewsCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
