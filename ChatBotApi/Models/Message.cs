namespace ChatBotApi.Models;

public enum MessageAuthor
{
    User,
    Bot
}

public class Message
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
    public MessageAuthor Author { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public Rating? Rating { get; set; }
}
