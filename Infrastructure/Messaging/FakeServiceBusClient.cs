using System.Text.Json;
using TransactionMicroservice.Domain.Interfaces;

namespace TransactionMicroservice.Infrastructure.Messaging;

public class FakeServiceBusClient : ITransactionQueueService
{
    public Task SendTransactionAsync<T>(T transaction, string queueName)
    {
        var json = JsonSerializer.Serialize(transaction);
        Console.WriteLine($"[FakeQueue] Enviando para fila '{queueName}': {json}");
        return Task.CompletedTask;
    }
}