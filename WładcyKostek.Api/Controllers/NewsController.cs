﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using WładcyKostek.Core.Requests.Queries;

namespace WładcyKostek.Api.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetNewsIds()
        {
            var response = await _mediator.Send(new GetNewsIdsQuery());
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNews(int id)
        {
            var response = await _mediator.Send(new GetNewsQuery { Id = id });
            return response.ErrorCode != System.Net.HttpStatusCode.OK ? NotFound(response) : Ok(response);
        }
    }
}
