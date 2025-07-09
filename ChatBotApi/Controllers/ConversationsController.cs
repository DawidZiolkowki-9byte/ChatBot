using ChatBotApi.Data;
using ChatBotApi.Features.Conversations;
using ChatBotApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Conversation>> Get(int id)
    {
        var convo = await _mediator.Send(new GetConversationQuery(id));
        if (convo == null)
            return NotFound();
        return convo;
    }

    [HttpPost]
    public async Task<ActionResult<Conversation>> Create()
    {
        var convo = await _mediator.Send(new CreateConversationCommand());
        return CreatedAtAction(nameof(Get), new { id = convo.Id }, convo);
    }
}
