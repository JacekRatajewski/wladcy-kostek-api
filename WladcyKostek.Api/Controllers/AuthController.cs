using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WladcyKostek.Core.Requests.Commands;
using WladcyKostek.Core.Requests.Queries;

namespace WladcyKostek.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("google")]
        public async Task<IActionResult> PostGoogleLogin([FromBody] LoginGoogleCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [HttpPost("register")]
        public async Task<IActionResult> PostRegister([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
