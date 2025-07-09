using ChatBotApi.Data;
using ChatBotApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Features.Conversations;

public record GetConversationQuery(int Id) : IRequest<Conversation?>;

public class GetConversationHandler : IRequestHandler<GetConversationQuery, Conversation?>
{
    private readonly ChatBotContext _context;

    public GetConversationHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<Conversation?> Handle(GetConversationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .ThenInclude(m => m.Rating)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
    }
}
