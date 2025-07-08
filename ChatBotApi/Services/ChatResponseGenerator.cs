namespace ChatBotApi.Services;

public class ChatResponseGenerator
{
    private readonly string[] _shortReplies =
    {
        "Lorem ipsum dolor sit amet.",
        "Consectetur adipiscing elit.",
        "Sed do eiusmod tempor."
    };

    private readonly string[] _mediumReplies =
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum.",
        "Cras vehicula, mi eget laoreet ullamcorper, metus nisi faucibus erat, vitae lacinia justo nulla in augue.",
        "Donec at velit varius, aliquet risus a, semper orci."
    };

    private readonly string[] _longReplies =
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur volutpat magna vel libero interdum, eget aliquet arcu consequat. Sed ac augue eget lorem posuere sollicitudin sed id ligula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Etiam at elit sed ipsum tempor pulvinar at sit amet elit.",
        "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Donec euismod, nisl eget consectetur porttitor, nisl nunc auctor ligula, vel gravida quam nisl sit amet est. In hac habitasse platea dictumst. Donec consectetur, enim a luctus fermentum, velit mi maximus massa, id euismod justo metus vitae arcu.",
        "Phasellus at orci id metus consequat lobortis. Fusce vitae consectetur mi. Sed sed risus semper, tempor odio vitae, suscipit nisl. Phasellus convallis turpis quis velit bibendum, nec cursus nulla elementum."
    };

    private readonly Random _random = new();

    public async IAsyncEnumerable<string> GenerateResponseAsync(CancellationToken token)
    {
        var choice = _random.Next(3);
        var response = choice switch
        {
            0 => _shortReplies[_random.Next(_shortReplies.Length)],
            1 => _mediumReplies[_random.Next(_mediumReplies.Length)],
            _ => _longReplies[_random.Next(_longReplies.Length)]
        };

        foreach (var ch in response)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(50, token); // simulate typing delay
            yield return ch.ToString();
        }
    }
}
