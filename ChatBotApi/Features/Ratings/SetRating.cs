using ChatBotApi.Data;
using ChatBotApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Features.Ratings;

public record SetRatingCommand(int MessageId, int Value) : IRequest<Rating?>;

public class SetRatingHandler : IRequestHandler<SetRatingCommand, Rating?>
{
    private readonly ChatBotContext _context;

    public SetRatingHandler(ChatBotContext context)
    {
        _context = context;
    }

    public async Task<Rating?> Handle(SetRatingCommand request, CancellationToken cancellationToken)
    {
        if (request.Value != 1 && request.Value != -1)
            return null;

        var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.MessageId == request.MessageId, cancellationToken);
        if (rating == null)
        {
            rating = new Rating { MessageId = request.MessageId, Value = request.Value };
            _context.Ratings.Add(rating);
        }
        else
        {
            rating.Value = request.Value;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return rating;
    }
}
