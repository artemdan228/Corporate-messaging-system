using Itmo.ObjectOrientedProgramming.Lab2.Archiver;
using Itmo.ObjectOrientedProgramming.Lab2.Core;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees;

public class ArchiverAddressee : IAddressee
{
    private readonly IArchiver _archiver;

    public ArchiverAddressee(IArchiver archiver)
    {
        _archiver = archiver;
    }

    public void ReceiveMessage(Message message)
    {
        _archiver.Archive(message);
    }
}