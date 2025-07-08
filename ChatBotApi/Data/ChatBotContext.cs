using ChatBotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApi.Data;

public class ChatBotContext : DbContext
{
    public ChatBotContext(DbContextOptions<ChatBotContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Rating> Ratings => Set<Rating>();
}
