using TransactionMicroservice.Domain.Entities;

namespace TransactionMicroservice.Domain.Interfaces;

public interface ITransactionQueueService
{
    Task SendTransactionAsync<T>(T transaction);
}