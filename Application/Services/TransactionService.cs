using TransactionMicroservice.Application.DTOs;
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
    
    public async Task<TransactionDto> CreateTransaction(CreateTransactionDto createTransactionDto)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Amount = createTransactionDto.Amount,
            Type = createTransactionDto.Type,
            Status = TransactionStatus.Pending,
            Sender = createTransactionDto.Sender,
            Receiver = createTransactionDto.Receiver,
        };
        
        await _repository.CreateAsync(transaction);
        
        await _queueService.SendTransactionAsync(transaction, "transactions-queue");

        var transactionDto = new TransactionDto
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Status = TransactionStatus.Pending,
            Sender = transaction.Sender,
            Receiver = transaction.Receiver,
        };
        
        return transactionDto;
    }
    
    public async Task<List<TransactionDto>> GetAllTransactions()
    {
        var transactions = await _repository.GetAllTransactionsAsync();
        
        var listTransactionsDto = transactions.Select(transaction => new TransactionDto
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Status = transaction.Status,
            Sender = transaction.Sender,
            Receiver = transaction.Receiver,
        }).ToList();

        return listTransactionsDto;
    }
}