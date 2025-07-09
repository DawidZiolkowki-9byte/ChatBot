using ChatBotApi.Data;
using ChatBotApi.Features.Ratings;
using ChatBotApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public class RatingRequest
    {
        public int MessageId { get; set; }
        public int Value { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> SetRating([FromBody] RatingRequest request)
    {
        if (request.Value != 1 && request.Value != -1)
            return BadRequest();

        var rating = await _mediator.Send(new SetRatingCommand(request.MessageId, request.Value));
        if (rating == null)
            return BadRequest();
        return Ok(rating);
    }
}
