using Moq;
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

    // Mock do Service
    public TransactionServiceTests()
    {
        _repositoryMock = new Mock<ITransactionRepository>();
        _queueServiceMock = new Mock<ITransactionQueueService>();
        _transactionService = new TransactionService(_repositoryMock.Object, _queueServiceMock.Object);
    }

    [Fact]
    public async Task CreateTransaction_WithNegativeAmount_ShouldThrow()
    {
        // Cria uma Transaction Invalida
        var transaction = new CreateTransactionDto()
        {
            Amount = -10,
            Type = TransactionType.Credit,
            Sender = "lucas",
            Receiver = "maria",
        };
        
        // Verifica se gera uma exceção ao tentar criar um Transaction com o amount negativo
        await Assert.ThrowsAsync<ArgumentException>(() => _transactionService.CreateTransaction(transaction));
    }

    [Fact]
    public async Task CreateTransaction_WithValidAmount_ShouldCallRepository()
    {
        // Cria uma Transaction Valida
        var dto = new CreateTransactionDto
        {
            Amount = 100, // Amount valido
            Type = TransactionType.Credit,
            Sender = "lucas",
            Receiver = "matos"
        };
        
        // Chama a funcao
        var result = await _transactionService.CreateTransaction(dto);

        // Verifica se chama corretamente o repositorio e serviço de fila
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Transaction>()), Times.Once);
        _queueServiceMock.Verify(q => q.SendTransactionAsync(It.IsAny<TransactionMessageDto>()), Times.Once);
        result.Should().NotBeNull();
        
    }
    
    [Fact]
    public async Task GetAllTransactions_ShouldReturnMappedList()
    {
        // mocka o repositório para retornar dados falsos
        var fakeTransactions = new List<Transaction>
        {
            new Transaction(100m, TransactionType.Credit, "Patricia", "Mario"),
            new Transaction(200m, TransactionType.Debit, "Lucas", "Maria"),
            new Transaction(0.50m, TransactionType.Debit, "Luan", "Gene")
        };

        _repositoryMock.Setup(r => r.GetAllTransactionsAsync())
            .ReturnsAsync(fakeTransactions);
        
        // Chama a função
        var result = await _transactionService.GetAllTransactions();

        // Esperado
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        
        Assert.Equal("Patricia", result[0].Sender);
        Assert.Equal("Mario", result[0].Receiver);
        Assert.Equal(100, result[0].Amount);
        Assert.Equal(TransactionStatus.Pending, result[0].Status);
        Assert.Equal(TransactionType.Credit, result[0].Type);
        
        Assert.Equal("Lucas", result[1].Sender);
        Assert.Equal("Maria", result[1].Receiver);
        Assert.Equal(200, result[1].Amount);
        Assert.Equal(TransactionStatus.Pending, result[1].Status);
        Assert.Equal(TransactionType.Debit, result[1].Type);
        
        Assert.Equal("Luan", result[2].Sender);
        Assert.Equal("Gene", result[2].Receiver);
        Assert.Equal(0.50m, result[2].Amount);
        Assert.Equal(TransactionStatus.Pending, result[2].Status);
        Assert.Equal(TransactionType.Debit, result[2].Type);
    }
    
    [Fact]
    public async Task GetAllTrasactions_ShouldReturnEmptyList_WhenNoTransactionsFound()
    {
        _repositoryMock.Setup(r => r.GetAllTransactionsAsync())
            .ReturnsAsync([]);
        
        var result = await _transactionService.GetAllTransactions();
        
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}