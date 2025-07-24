using Moq;
using TransactionMicroservice.Infrastructure.Messaging;
using Azure.Messaging.ServiceBus;

namespace TransactionMicroservice.Tests.Infrastructure;

public class MessageSenderServiceTests
{
    [Fact]
    public async Task SendTransactionAsync_ShouldCallSendMessageAsync()
    {
        //Mock
        var mockSender = new Mock<ServiceBusSender>();
        mockSender
            .Setup(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var service = new MessageSenderService(mockSender.Object);

        var transaction = new { Amount = 100, From = "Alice", To = "Bob" };

        // Chama a funcao
        await service.SendTransactionAsync(transaction);

        // Esperado
        mockSender.Verify(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}