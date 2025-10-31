using Itmo.ObjectOrientedProgramming.Lab2.Addressees;

namespace Itmo.ObjectOrientedProgramming.Lab2.Core;

public class Topic
{
    public string Name { get; set; }

    private readonly List<IAddressee> _addressees = new();

    public Topic(string name)
    {
        Name = name;
    }

    public void AddAddressee(IAddressee addressee)
    {
        _addressees.Add(addressee);
    }

    public void SendMessage(Message message)
    {
        foreach (IAddressee addressee in _addressees)
            addressee.ReceiveMessage(message);
    }
}