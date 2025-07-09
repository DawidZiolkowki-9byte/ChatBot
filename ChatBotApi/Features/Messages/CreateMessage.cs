using ChatBotApi.Data;
using ChatBotApi.Models;
using MediatR;

namespace ChatBotApi.Features.Messages;

public record CreateMessageCommand(int ConversationId, MessageAuthor Author, string Content) : IRequest<Message?>;

public class CreateMessageHandler : IRequestHandler<CreateMessageCommand, Message?>
{
    private readonly ChatBotContext _context;

    public CreateMessageHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<Message?> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var convo = await _context.Conversations.FindAsync(new object?[] { request.ConversationId }, cancellationToken: cancellationToken);
        if (convo == null)
        {
            return null;
        }

        var msg = new Message
        {
            ConversationId = request.ConversationId,
            Author = request.Author,
            Content = request.Content,
            Created = DateTime.UtcNow
        };
        _context.Messages.Add(msg);
        await _context.SaveChangesAsync(cancellationToken);
        return msg;
    }
}
