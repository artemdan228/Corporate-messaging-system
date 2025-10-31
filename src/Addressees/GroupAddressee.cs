using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees;

public class GroupAddressee : IAddressee
{
    private readonly List<IAddressee> _addressees = new();

    public void AddAddressee(IAddressee addressee)
    {
        _addressees.Add(addressee);
    }

    public void ReceiveMessage(Message message)
    {
        foreach (IAddressee addressee in _addressees)
        {
            addressee.ReceiveMessage(message);
        }
    }
}