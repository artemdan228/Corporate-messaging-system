namespace Itmo.ObjectOrientedProgramming.Lab2.AlertSystem;

public class TextAlertSystem : IAlertSystem
{
    private readonly string _message;

    public TextAlertSystem(string message)
    {
        _message = message;
    }

    public void Alert()
    {
        Console.WriteLine(_message);
    }
}