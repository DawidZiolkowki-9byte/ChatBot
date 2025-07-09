using ChatBotApi.Data;
using ChatBotApi.Features.Messages;
using ChatBotApi.Models;
using ChatBotApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ChatBotContext _context;
    private readonly IMediator _mediator;
    private readonly ChatResponseGenerator _generator;

    public MessagesController(ChatBotContext context, IMediator mediator, ChatResponseGenerator generator)
    {
        _context = context;
        _mediator = mediator;
        _generator = generator;
    }

    public class SendMessageRequest
    {
        public int ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var token = HttpContext.RequestAborted;
        var userMessage = await _mediator.Send(new CreateMessageCommand(request.ConversationId, MessageAuthor.User, request.Content), token);
        if (userMessage == null)
            return NotFound();

        var botMessage = await _mediator.Send(new CreateMessageCommand(request.ConversationId, MessageAuthor.Bot, string.Empty), token);
        if (botMessage == null)
            return NotFound();

        Response.ContentType = "text/plain";
        await foreach (var ch in _generator.GenerateResponseAsync(token))
        {
            botMessage.Content += ch;
            await Response.WriteAsync(ch, token);
            await Response.Body.FlushAsync(token);
        }

        await _context.SaveChangesAsync(token);
        return new EmptyResult();
    }
}
