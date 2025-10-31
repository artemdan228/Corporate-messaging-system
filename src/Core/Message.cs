using Itmo.ObjectOrientedProgramming.Lab2.ImportanceLevels;

namespace Itmo.ObjectOrientedProgramming.Lab2.Core;

public class Message
{
    public string Title { get; }

    public string Body { get; }

    public IImportanceLevel Importance { get; }

    public Message(string title, string body, IImportanceLevel importance)
    {
        Title = title;
        Body = body;
        Importance = importance;
    }
}