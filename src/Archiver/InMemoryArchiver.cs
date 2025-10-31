using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Archiver;

public class InMemoryArchiver : IArchiver
{
    private readonly List<Message> _messages = new();

    public void Archive(Message message)
    {
        _messages.Add(message);
    }

    public IEnumerable<Message> GetMessages()
    {
        return _messages;
    }
}