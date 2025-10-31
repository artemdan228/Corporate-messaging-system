using Itmo.ObjectOrientedProgramming.Lab2.Addressees;
using Itmo.ObjectOrientedProgramming.Lab2.Addressees.Processing;
using Itmo.ObjectOrientedProgramming.Lab2.AlertSystem;
using Itmo.ObjectOrientedProgramming.Lab2.Archiver;
using Itmo.ObjectOrientedProgramming.Lab2.Archiver.Formatters;
using Itmo.ObjectOrientedProgramming.Lab2.Core;
using Itmo.ObjectOrientedProgramming.Lab2.ImportanceLevels;
using Itmo.ObjectOrientedProgramming.Lab2.MessageStatuses;
using Moq;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class MessageSystemTests
{
    // Тест: при получении сообщения пользователем, оно сохраняется в статусе "не прочитано"
    [Fact]
    public void ReceiveMessage_MessageStoredAsUnread()
    {
        var user = new User();
        var message = new Message("Test", "Body", new NormalImportance());

        user.ReceiveMessage(message);

        Assert.IsType<UnreadStatus>(user.GetStatus(message));
    }

    // Тест: при попытке отметить сообщение пользователя в статусе "не прочитано" как прочитанное, оно должно поменять свой статус
    [Fact]
    public void MarkAsRead_FromUnreadToRead_Success()
    {
        var user = new User();
        var message = new Message("Test", "Body", new NormalImportance());
        user.ReceiveMessage(message);

        bool result = user.MarkAsRead(message);

        Assert.True(result);
        Assert.IsType<ReadStatus>(user.GetStatus(message));
    }

    // Тест: при попытке отметить сообщение пользователя в статусе "прочитано" как прочитанное, должна вернуться ошибка
    [Fact]
    public void MarkAsRead_AlreadyReadMessage_ThrowsException()
    {
        var user = new User();
        var message = new Message("Test", "Body", new NormalImportance());
        user.ReceiveMessage(message);
        user.MarkAsRead(message);

        Assert.Throws<InvalidOperationException>(() => user.MarkAsRead(message));
    }

    // Тест: при настроенном фильтре для адресата, отправленное сообщение, не подходящее под критерии важности - до адресата дойти не должно
    [Fact]
    public void ImportanceFilterAddressee_MessageImportanceBelowThreshold_MessageNotReceived()
    {
        var mockAddressee = new Mock<IAddressee>();
        var filter = new ImportanceFilterAddresseeDecorator(mockAddressee.Object, new HighImportance());
        var lowPriorityMessage = new Message("Low", "Body", new NormalImportance());

        filter.ReceiveMessage(lowPriorityMessage);

        mockAddressee.Verify(a => a.ReceiveMessage(It.IsAny<Message>()), Times.Never());
    }

    // Тест: при настроенном фильтре для адресата, сообщение с достаточной важностью доходит до адресата
    [Fact]
    public void ImportanceFilterAddressee_MessageImportanceMeetsThreshold_MessageReceived()
    {
        var mockAddressee = new Mock<IAddressee>();
        var filter = new ImportanceFilterAddresseeDecorator(mockAddressee.Object, new HighImportance());
        var highPriorityMessage = new Message("High", "Body", new HighImportance());

        filter.ReceiveMessage(highPriorityMessage);

        mockAddressee.Verify(a => a.ReceiveMessage(highPriorityMessage), Times.Once());
    }

    // Тест: при настроенном логгировании адресата, должен писаться лог, когда приходит сообщение
    [Fact]
    public void LoggingAddresseeDecorator_WhenMessageReceived_LogsMessage()
    {
        var mockAddressee = new Mock<IAddressee>();
        var mockLogger = new Mock<ILogger>();
        var loggingDecorator = new LoggingAddresseeDecorator(mockAddressee.Object, mockLogger.Object);
        var message = new Message("Test", "Body", new NormalImportance());

        loggingDecorator.ReceiveMessage(message);

        mockLogger.Verify(l => l.Log($"Received message: {message.Title}"), Times.Once());
        mockAddressee.Verify(a => a.ReceiveMessage(message), Times.Once());
    }

    // Тест: при сохранении сообщения в форматирующий архиватор, его реализация должна вызывать ожидаемые действия
    [Fact]
    public void FormattingArchiver_WhenMessageArchived_FormattersAreCalled()
    {
        var mockFormatter = new Mock<IMessageFormatter>();
        mockFormatter.Setup(f => f.FormatTitle(It.IsAny<string>())).Returns("Formatted Title");
        mockFormatter.Setup(f => f.FormatBody(It.IsAny<string>())).Returns("Formatted Body");
        var archiver = new FormattingArchiver(mockFormatter.Object);
        var message = new Message("Test", "Body", new NormalImportance());

        archiver.Archive(message);

        mockFormatter.Verify(f => f.FormatTitle("Test"), Times.Once());
        mockFormatter.Verify(f => f.FormatBody("Body"), Times.Once());
    }

    // Тест: добавляются два адресата-пользователя (для одного пользователя), для одного из них настраивается фильтр важности,
    // при попытке отправить сообщение с важностью ниже настроенной - пользователь получает значение единожды
    [Fact]
    public void GroupAddressee_WithFilteredAndUnfilteredUsers_SendsMessageAppropriately()
    {
        var user = new User();
        var userAddressee = new UserAddressee(user);
        var filteredAddressee = new ImportanceFilterAddresseeDecorator(userAddressee, new HighImportance());
        var group = new GroupAddressee();
        group.AddAddressee(userAddressee);
        group.AddAddressee(filteredAddressee);

        var normalMessage = new Message("Normal", "Body", new NormalImportance());
        var highMessage = new Message("High", "Body", new HighImportance());

        group.ReceiveMessage(normalMessage);
        Assert.IsType<UnreadStatus>(user.GetStatus(normalMessage));

        group.ReceiveMessage(highMessage);
        Assert.IsType<UnreadStatus>(user.GetStatus(highMessage));
    }

    // Тест: система оповещений срабатывает при обнаружении подозрительных слов в сообщении
    [Fact]
    public void AlertSystemAddressee_WithSuspiciousWord_TriggersAlert()
    {
        var mockAlertSystem = new Mock<IAlertSystem>();
        var alertAddressee = new AlertSystemAddressee(mockAlertSystem.Object);
        alertAddressee.AddSuspiciousWord("hack");

        var suspiciousMessage = new Message("Alert", "Possible hack attempt", new HighImportance());
        var normalMessage = new Message("Normal", "Regular update", new NormalImportance());

        alertAddressee.ReceiveMessage(suspiciousMessage);
        mockAlertSystem.Verify(a => a.Alert(), Times.Once());

        alertAddressee.ReceiveMessage(normalMessage);
        mockAlertSystem.Verify(a => a.Alert(), Times.Once());
    }

    // Тест: сообщение сохраняется в архиваторе в памяти
    [Fact]
    public void InMemoryArchiver_ArchiveMessage_StoresMessageInCollection()
    {
        var archiver = new InMemoryArchiver();
        var message = new Message("Test", "Body", new NormalImportance());

        archiver.Archive(message);

        IEnumerable<Message> messages = archiver.GetMessages();
        Assert.Single(messages);
        Assert.Equal(message, messages.First());
    }

    // Тест: топик доставляет сообщение всем своим адресатам
    [Fact]
    public void Topic_SendMessage_DeliversToAllAddressees()
    {
        var topic = new Topic("Test Topic");
        var mockAddressee1 = new Mock<IAddressee>();
        var mockAddressee2 = new Mock<IAddressee>();
        topic.AddAddressee(mockAddressee1.Object);
        topic.AddAddressee(mockAddressee2.Object);
        var message = new Message("Test", "Body", new NormalImportance());

        topic.SendMessage(message);

        mockAddressee1.Verify(a => a.ReceiveMessage(message), Times.Once());
        mockAddressee2.Verify(a => a.ReceiveMessage(message), Times.Once());
    }
}