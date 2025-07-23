using TransactionMicroservice.Domain.Entities;

namespace TransactionMicroservice.Domain.Interfaces;

public interface ITransactionRepository
{
    Task CreateAsync(Transaction transaction);
    Task<List<Transaction>> GetAllTransactionsAsync();
}