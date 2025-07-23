using Microsoft.AspNetCore.Mvc;
using TransactionMicroservice.Application.DTOs;
using TransactionMicroservice.Application.Services;
using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class TransactionController: ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Date = dto.Date,
            Amount = dto.Amount,
            Type = dto.Type,
            Status = TransactionStatus.Pending,
            Sender = dto.Sender,
            Receiver = dto.Receiver,
        };

        await _transactionService.CreateTransaction(transaction);

        var response = new TransactionDto
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Status = transaction.Status,
            Sender = transaction.Sender,
            Receiver = transaction.Receiver,
        };
        
        return CreatedAtAction(nameof(GetAllTransactions), new { id = transaction.Id }, response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllTransactions();
        
        var response = transactions.Select(transaction => new TransactionDto
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Status = transaction.Status,
            Sender = transaction.Sender,
            Receiver = transaction.Receiver,
        }).ToList();
        
        return Ok(response);
    }
}