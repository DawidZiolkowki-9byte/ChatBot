using ChatBotApi.Data;
using ChatBotApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly ChatBotContext _context;

    public ConversationsController(ChatBotContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Conversation>> Get(int id)
    {
        var convo = await _context.Conversations
            .Include(c => c.Messages)
            .ThenInclude(m => m.Rating)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (convo == null)
            return NotFound();
        return convo;
    }

    [HttpPost]
    public async Task<ActionResult<Conversation>> Create()
    {
        var convo = new Conversation();
        _context.Conversations.Add(convo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = convo.Id }, convo);
    }
}
