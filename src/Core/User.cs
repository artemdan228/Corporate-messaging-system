using Itmo.ObjectOrientedProgramming.Lab2.MessageStatuses;

namespace Itmo.ObjectOrientedProgramming.Lab2.Core;

public class User
{
    private readonly Dictionary<Message, IMessageStatus> _receivedMessages = new();

    public void ReceiveMessage(Message message)
    {
        _receivedMessages[message] = new UnreadStatus();
    }

    public bool MarkAsRead(Message message)
    {
        if (!_receivedMessages.ContainsKey(message))
            return false;

        if (_receivedMessages[message] is ReadStatus)
            throw new InvalidOperationException("Message already marked as read");

        _receivedMessages[message] = new ReadStatus();
        return true;
    }

    public IMessageStatus GetStatus(Message message)
    {
        return _receivedMessages[message];
    }
}