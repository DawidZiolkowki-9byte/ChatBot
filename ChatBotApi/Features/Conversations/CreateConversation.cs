using ChatBotApi.Data;
using ChatBotApi.Models;
using MediatR;

namespace ChatBotApi.Features.Conversations;

public record CreateConversationCommand() : IRequest<Conversation>;

public class CreateConversationHandler : IRequestHandler<CreateConversationCommand, Conversation>
{
    private readonly ChatBotContext _context;

    public CreateConversationHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<Conversation> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var convo = new Conversation
        {
            Title = $"Conversation {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}"
        };
        _context.Conversations.Add(convo);
        await _context.SaveChangesAsync(cancellationToken);
        return convo;
    }
}
