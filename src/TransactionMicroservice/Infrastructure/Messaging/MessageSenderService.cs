using Azure.Messaging.ServiceBus;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Helpers;

namespace TransactionMicroservice.Infrastructure.Messaging;

public class MessageSenderService : ITransactionQueueService
{
    private readonly ServiceBusSender _sender;

    public MessageSenderService(ServiceBusSender sender)
    {
        _sender = sender;
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