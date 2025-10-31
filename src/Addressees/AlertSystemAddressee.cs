using Itmo.ObjectOrientedProgramming.Lab2.AlertSystem;
using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees;

public class AlertSystemAddressee : IAddressee
{
    private readonly IAlertSystem _alertSystem;

    private readonly List<string> _suspiciousWords;

    public AlertSystemAddressee(IAlertSystem alertSystem)
    {
        _alertSystem = alertSystem;
        _suspiciousWords = new List<string>();
    }

    public void AddSuspiciousWord(string word)
    {
        _suspiciousWords.Add(word);
    }

    public void ReceiveMessage(Message message)
    {
        foreach (string word in _suspiciousWords)
        {
            if (message.Body.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                _alertSystem.Alert();
                break;
            }
        }
    }
}