using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Enums;
using TransactionMicroservice.Domain.Interfaces;

namespace TransactionMicroservice.Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly ITransactionQueueService _queueService;
    
    public TransactionService(
        ITransactionRepository repository,
        ITransactionQueueService queueService)
    {
        _repository = repository;
        _queueService = queueService;
    }
    
    public async Task CreateTransaction(Transaction transaction)
    {
        transaction.Date = DateTime.UtcNow;
        transaction.Status = TransactionStatus.Pending;
        
        await _repository.CreateAsync(transaction);
        
        await _queueService.SendTransactionAsync(transaction, "transactions-queue");
    }
    
    public async Task<List<Transaction>> GetAllTransactions()
    {
        return await _repository.GetAllTransactionsAsync();
    }
}