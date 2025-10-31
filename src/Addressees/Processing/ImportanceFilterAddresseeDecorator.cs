using Itmo.ObjectOrientedProgramming.Lab2.Core;
using Itmo.ObjectOrientedProgramming.Lab2.ImportanceLevels;

namespace Itmo.ObjectOrientedProgramming.Lab2.Addressees.Processing;

public class ImportanceFilterAddresseeDecorator : IAddressee
{
    private readonly IAddressee _inner;
    private readonly IImportanceLevel _minImportance;

    public ImportanceFilterAddresseeDecorator(IAddressee inner, IImportanceLevel minImportance)
    {
        _inner = inner;
        _minImportance = minImportance;
    }

    public void ReceiveMessage(Message message)
    {
        if (IsImportantEnough(message.Importance))
            _inner.ReceiveMessage(message);
    }

    private bool IsImportantEnough(IImportanceLevel importance)
    {
        var importanceOrder = new Dictionary<string, int>
        {
            ["Low"] = 1,
            ["Normal"] = 2,
            ["High"] = 3,
        };

        return importanceOrder[importance.Name] >= importanceOrder[_minImportance.Name];
    }
}