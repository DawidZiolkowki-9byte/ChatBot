namespace ChatBotApi.Models.Dto;

public class ConversationDto
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string? Title { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
}