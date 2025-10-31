namespace Itmo.ObjectOrientedProgramming.Lab2.Archiver.Formatters;

public interface IMessageFormatter
{
    string FormatTitle(string title);

    string FormatBody(string body);
}