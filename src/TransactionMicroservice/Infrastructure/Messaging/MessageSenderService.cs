using Azure.Messaging.ServiceBus;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Helpers;

namespace TransactionMicroservice.Infrastructure.Messaging;

public class MessageSenderService : ITransactionQueueService
{
    private readonly string _connectionString;
    private readonly string _queueName;
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public MessageSenderService(IConfiguration config)
    {
        _connectionString = config["ServiceBus:ConnectionString"];
        _queueName = config["ServiceBus:QueueName"];
        
        _client = new ServiceBusClient(_connectionString);
        _sender = _client.CreateSender(_queueName);
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