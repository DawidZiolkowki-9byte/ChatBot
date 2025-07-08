namespace ChatBotApi.Models;

public class Conversation
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string? Title { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
