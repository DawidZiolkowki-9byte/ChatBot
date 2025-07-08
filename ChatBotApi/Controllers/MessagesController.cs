using ChatBotApi.Data;
using ChatBotApi.Models;
using ChatBotApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ChatBotContext _context;
    private readonly ChatResponseGenerator _generator;

    public MessagesController(ChatBotContext context, ChatResponseGenerator generator)
    {
        _context = context;
        _generator = generator;
    }

    public class SendMessageRequest
    {
        public int ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class MessageChunk
    {
        public string Content { get; set; } = string.Empty;
    }

    [HttpPost]
    public async IAsyncEnumerable<MessageChunk> SendMessage([FromBody] SendMessageRequest request, CancellationToken token)
    {
        var convo = await _context.Conversations.FindAsync(new object?[] { request.ConversationId }, cancellationToken: token);
        if (convo == null)
        {
            yield break;
        }
        var userMessage = new Message
        {
            ConversationId = request.ConversationId,
            Author = MessageAuthor.User,
            Content = request.Content,
            Created = DateTime.UtcNow
        };
        _context.Messages.Add(userMessage);
        await _context.SaveChangesAsync(token);

        var botMessage = new Message
        {
            ConversationId = request.ConversationId,
            Author = MessageAuthor.Bot,
            Content = string.Empty,
            Created = DateTime.UtcNow
        };
        _context.Messages.Add(botMessage);
        await _context.SaveChangesAsync(token);

        await foreach (var ch in _generator.GenerateResponseAsync(token))
        {
            botMessage.Content += ch;
            yield return new MessageChunk { Content = ch };
        }

        await _context.SaveChangesAsync(token);
    }
}
