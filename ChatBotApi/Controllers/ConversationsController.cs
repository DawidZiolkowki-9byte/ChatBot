using ChatBotApi.Data;
using ChatBotApi.Features.Conversations;
using ChatBotApi.Models;
using ChatBotApi.Models.Dto;
using System.Linq;
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

    private static ConversationDto ToDto(Conversation convo)
    {
        return new ConversationDto
        {
            Id = convo.Id,
            Created = convo.Created,
            Title = convo.Title,
            Messages = convo.Messages.Select(m => new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                Author = m.Author,
                Content = m.Content,
                Created = m.Created,
                Rating = m.Rating == null ? null : new RatingDto
                {
                    Id = m.Rating.Id,
                    MessageId = m.Rating.MessageId,
                    Value = m.Rating.Value
                }
            }).ToList()
        };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConversationDto>>> GetAll()
    {
        var convos = await _mediator.Send(new ListConversationsQuery());
        return convos.Select(ToDto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConversationDto>> Get(int id)
    {
        var convo = await _mediator.Send(new GetConversationQuery(id));
        if (convo == null)
            return NotFound();
        return ToDto(convo);
    }

    [HttpPost]
    public async Task<ActionResult<ConversationDto>> Create()
    {
        var convo = await _mediator.Send(new CreateConversationCommand());
        return CreatedAtAction(nameof(Get), new { id = convo.Id }, ToDto(convo));
    }

    [HttpDelete]
    public async Task<IActionResult> Clear()
    {
        await _mediator.Send(new ClearConversationsCommand());
        return NoContent();
    }
}
