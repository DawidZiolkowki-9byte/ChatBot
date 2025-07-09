using ChatBotApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Features.Conversations;

public record ClearConversationsCommand() : IRequest<Unit>;

public class ClearConversationsHandler : IRequestHandler<ClearConversationsCommand, Unit>
{
    private readonly ChatBotContext _context;
    public ClearConversationsHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ClearConversationsCommand request, CancellationToken cancellationToken)
    {
        _context.Ratings.RemoveRange(_context.Ratings);
        _context.Messages.RemoveRange(_context.Messages);
        _context.Conversations.RemoveRange(_context.Conversations);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}