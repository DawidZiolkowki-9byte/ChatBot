using ChatBotApi.Data;
using ChatBotApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly ChatBotContext _context;

    public RatingsController(ChatBotContext context)
    {
        _context = context;
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

        var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.MessageId == request.MessageId);
        if (rating == null)
        {
            rating = new Rating { MessageId = request.MessageId, Value = request.Value };
            _context.Ratings.Add(rating);
        }
        else
        {
            rating.Value = request.Value;
        }

        await _context.SaveChangesAsync();
        return Ok(rating);
    }
}
