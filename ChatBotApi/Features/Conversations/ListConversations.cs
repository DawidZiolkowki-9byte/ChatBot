using ChatBotApi.Data;
using ChatBotApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Features.Conversations;

public record ListConversationsQuery() : IRequest<List<Conversation>>;

public class ListConversationsHandler : IRequestHandler<ListConversationsQuery, List<Conversation>>
{
    private readonly ChatBotContext _context;

    public ListConversationsHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<List<Conversation>> Handle(ListConversationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .ThenInclude(m => m.Rating)
            .OrderBy(c => c.Created)
            .ToListAsync(cancellationToken);
    }
}