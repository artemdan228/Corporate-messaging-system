namespace Itmo.ObjectOrientedProgramming.Lab2.Archiver.Formatters;

public class ConsoleFormatter
{
    public string FormatTitle(string title)
    {
        return $"## {title}";
    }

    public string FormatBody(string body)
    {
        return $"**{body}**";
    }
}