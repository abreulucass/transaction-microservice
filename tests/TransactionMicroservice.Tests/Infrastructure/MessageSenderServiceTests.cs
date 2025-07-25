using Moq;
using TransactionMicroservice.Infrastructure.Messaging;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using TransactionMicroservice.Infrastructure.Configurations;

namespace TransactionMicroservice.Tests.Infrastructure;

public class MessageSenderServiceTests
{
    
    [Fact]
    public async Task SendTransactionAsync_ShouldCallSendMessageAsync()
    {
        //Mock
        var settings = Options.Create(new AzureBusServiceSettings
        {
            QueueName = "test-queue"
        });

        var mockSender = new Mock<ServiceBusSender>();
        mockSender
            .Setup(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var mockClient = new Mock<ServiceBusClient>();
        mockClient
            .Setup(c => c.CreateSender("test-queue"))
            .Returns(mockSender.Object);

        var service = new MessageSenderService(mockClient.Object, settings);

        var transaction = new { Amount = 100, From = "Alice", To = "Bob" };

        // Chama a funcao
        await service.SendTransactionAsync(transaction);

        // Esperado
        mockSender.Verify(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}