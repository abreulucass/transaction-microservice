using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Helpers;
using TransactionMicroservice.Infrastructure.Configurations;

namespace TransactionMicroservice.Infrastructure.Messaging;

public class MessageSenderService : ITransactionQueueService
{
    private readonly ServiceBusSender _sender;

    public MessageSenderService(ServiceBusClient client, IOptions<AzureBusServiceSettings> options)
    {
        var queueName = options.Value.QueueName;
        _sender = client.CreateSender(queueName);
    }
    
    public async Task SendTransactionAsync<T>(T transaction)
    {
        var json = JsonHelper.ToJson(transaction); 
        var message = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            Subject = "TransactionCreated"
        };
        
        await _sender.SendMessageAsync(message);
    }
}