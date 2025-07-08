namespace ChatBotApi.Models;

public class Rating
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public Message? Message { get; set; }
    public int Value { get; set; }
}
