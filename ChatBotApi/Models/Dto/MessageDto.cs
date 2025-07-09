using ChatBotApi.Models;

namespace ChatBotApi.Models.Dto;

public class MessageDto
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public MessageAuthor Author { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public RatingDto? Rating { get; set; }
}