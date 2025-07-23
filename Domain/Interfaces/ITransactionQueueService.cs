namespace TransactionMicroservice.Domain.Interfaces;

public interface ITransactionQueueService
{
    Task SendTransactionAsync<T>(T transaction, string queueName);
}