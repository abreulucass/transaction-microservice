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
        var transactionDto = await _transactionService.CreateTransaction(dto);

        var response = transactionDto;
        
        return CreatedAtAction(nameof(GetAllTransactions), new { id = transactionDto.Id }, response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllTransactions();
        
        return Ok(transactions);
    }
}