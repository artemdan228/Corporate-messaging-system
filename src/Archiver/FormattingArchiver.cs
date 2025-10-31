using Itmo.ObjectOrientedProgramming.Lab2.Archiver.Formatters;
using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Archiver;

public class FormattingArchiver : IArchiver
{
    private readonly IMessageFormatter _formatter;

    public FormattingArchiver(IMessageFormatter formatter)
    {
        _formatter = formatter;
    }

    public void Archive(Message message)
    {
        string formattedTitle = _formatter.FormatTitle(message.Title);
        string formattedBody = _formatter.FormatBody(message.Body);
        Console.WriteLine($"{formattedTitle}\n{formattedBody}");
    }
}