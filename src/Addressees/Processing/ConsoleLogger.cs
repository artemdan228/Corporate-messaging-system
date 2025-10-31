namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees.Processing;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss}: {message}");
    }
}