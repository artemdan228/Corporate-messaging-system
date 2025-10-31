using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees;

public class UserAddressee : IAddressee
{
    private readonly User _user;

    public UserAddressee(User user)
    {
        _user = user;
    }

    public void ReceiveMessage(Message message)
    {
        _user.ReceiveMessage(message);
    }
}