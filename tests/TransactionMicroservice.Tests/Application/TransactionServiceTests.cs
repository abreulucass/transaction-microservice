using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using TransactionMicroservice.Application.DTOs;
using TransactionMicroservice.Application.Services;
using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Enums;
using TransactionMicroservice.Domain.Interfaces;

namespace TransactionMicroservice.Tests.Application;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _repositoryMock;
    private readonly Mock<ITransactionQueueService> _queueServiceMock;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _repositoryMock = new Mock<ITransactionRepository>();
        _queueServiceMock = new Mock<ITransactionQueueService>();
        _transactionService = new TransactionService(_repositoryMock.Object, _queueServiceMock.Object);
    }

    [Fact]
    public async Task CreateTransaction_WithNegativeAmount_ShouldThrow()
    {
        var transaction = new CreateTransactionDto()
        {
            Amount = -10,
            Type = TransactionType.Credit,
            Sender = "lucas",
            Receiver = "maria",
        };
        
        await Assert.ThrowsAsync<ArgumentException>(() => _transactionService.CreateTransaction(transaction));
    }

    [Fact]
    public async Task CreateTransaction_WithValidAmount_ShouldCallRepository()
    {
        // Arrange
        var dto = new CreateTransactionDto
        {
            Amount = 100, // positivo!
            Type = TransactionType.Credit,
            Sender = "lucas",
            Receiver = "matos"
        };

        // Act
        var result = await _transactionService.CreateTransaction(dto);

        // Assert
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Transaction>()), Times.Once);
        _queueServiceMock.Verify(q => q.SendTransactionAsync(It.IsAny<TransactionMessageDto>()), Times.Once);
        result.Should().NotBeNull();

    }
}