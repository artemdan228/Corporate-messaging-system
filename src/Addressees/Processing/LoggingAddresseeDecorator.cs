using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees.Processing;

public class LoggingAddresseeDecorator : IAddressee
{
    private readonly IAddressee _inner;
    private readonly ILogger _logger;

    public LoggingAddresseeDecorator(IAddressee inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public void ReceiveMessage(Message message)
    {
        _logger.Log($"Received message: {message.Title}");
        _inner.ReceiveMessage(message);
    }
}