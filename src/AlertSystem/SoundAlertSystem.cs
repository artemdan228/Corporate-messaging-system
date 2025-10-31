namespace Itmo.ObjectOrientedProgramming.Lab2.AlertSystem;

public class SoundAlertSystem : IAlertSystem
{
    public void Alert()
    {
        Console.Beep();
    }
}