using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WladcyKostek.Core.Requests.Queries;

namespace WladcyKostek.Api.Controllers
{
    [ApiController]
    [Route("scrappedNews")]
    public class ScrappedNewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;

        public ScrappedNewsController(ILogger<NewsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var response = await _mediator.Send(new GetScrappedNewsTop6Query());
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpGet("more")]
        public async Task<IActionResult> GetMoreNews()
        {
            var response = await _mediator.Send(new GetScrappedNewsAllSkip6Query());
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpGet("all")]
        public async Task<IActionResult> GetNewsAll()
        {
            var response = await _mediator.Send(new GetScrappedNewsAllQuery());
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
