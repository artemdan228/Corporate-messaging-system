using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees;

public interface IAddressee
{
    void ReceiveMessage(Message message);
}