
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TransactionMicroservice.Infrastructure.Repositories;
using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Enums;
using TransactionMicroservice.Infrastructure.Configurations;

namespace TransactionMicroservice.Tests.Infrastructure;

public class DbTransactionRepositoryTests
{
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnEmptyList_WhenNoTransactionsExist()
    {
        // Mock
        var mockCursor = new Mock<IAsyncCursor<Transaction>>();
        mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.Setup(x => x.Current).Returns(new List<Transaction>());

        var mockCollection = new Mock<IMongoCollection<Transaction>>();
        mockCollection
            .Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Transaction>>(),
                It.IsAny<FindOptions<Transaction, Transaction>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        var mockDatabase = new Mock<IMongoDatabase>();
        mockDatabase
            .Setup(d => d.GetCollection<Transaction>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);

        var mockClient = new Mock<IMongoClient>();
        mockClient
            .Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(mockDatabase.Object);

        var mockSettings = Options.Create(new MongoDbSettings
        {
            DatabaseName = "TestDb",
            CollectionName = "Transactions"
        });

        var repository = new DbTransacationRepository(mockClient.Object, mockSettings);

        // Chama a funcao
        var result = await repository.GetAllTransactionsAsync();

        // Esperado
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCallInsertOneAsync()
    {
        // Mock
        var transaction = new Transaction(100m, TransactionType.Credit, "Teste", "Banco X");

        var mockCollection = new Mock<IMongoCollection<Transaction>>();
        mockCollection
            .Setup(c => c.InsertOneAsync(transaction, null, default))
            .Returns(Task.CompletedTask)
            .Verifiable(); // Marca que precisa ser chamado

        var mockDatabase = new Mock<IMongoDatabase>();
        mockDatabase
            .Setup(d => d.GetCollection<Transaction>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);

        var mockClient = new Mock<IMongoClient>();
        mockClient
            .Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(mockDatabase.Object);

        var mockSettings = Options.Create(new MongoDbSettings
        {
            DatabaseName = "TestDb",
            CollectionName = "Transactions"
        });

        var repository = new DbTransacationRepository(mockClient.Object, mockSettings);

        // Chama a funcao
        await repository.CreateAsync(transaction);

        // Esperado
        mockCollection.Verify(c => c.InsertOneAsync(transaction, null, default), Times.Once);
    }
    
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnMockedTransactions()
    {
        // Mock
        var mockedTransactions = new List<Transaction>
        {
            new Transaction(100m, TransactionType.Credit, "Lucas", "Maria"),
            new Transaction(50m, TransactionType.Debit, "Mario", "Patricia"),
        };

        // Mock do cursor retornando as transações
        var mockCursor = new Mock<IAsyncCursor<Transaction>>();
        mockCursor.Setup(_ => _.Current).Returns(mockedTransactions);
        mockCursor
            .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)   // primeira chamada: tem dados
            .ReturnsAsync(false); // segunda chamada: fim dos dados

        // Mock da coleção para retornar o cursor
        var mockCollection = new Mock<IMongoCollection<Transaction>>();
        mockCollection
            .Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Transaction>>(),
                It.IsAny<FindOptions<Transaction, Transaction>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Mock do banco de dados e cliente
        var mockDatabase = new Mock<IMongoDatabase>();
        mockDatabase
            .Setup(d => d.GetCollection<Transaction>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);

        var mockClient = new Mock<IMongoClient>();
        mockClient
            .Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(mockDatabase.Object);

        // Configuração do MongoDbSettings
        var mockSettings = Options.Create(new MongoDbSettings
        {
            DatabaseName = "TestDb",
            CollectionName = "Transactions"
        });

        var repository = new DbTransacationRepository(mockClient.Object, mockSettings);

        // Act
        var result = await repository.GetAllTransactionsAsync();

        // Esperado
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        
        Assert.Equal("Lucas", result[0].Sender);
        Assert.Equal("Maria", result[0].Receiver);
        Assert.Equal(100m, result[0].Amount);
        Assert.Equal(TransactionStatus.Pending, result[0].Status);
        Assert.Equal(TransactionType.Credit, result[0].Type);
        
        Assert.Equal("Patricia", result[1].Receiver);
        Assert.Equal("Mario", result[1].Sender);
        Assert.Equal(50m, result[1].Amount);
        Assert.Equal(TransactionStatus.Pending, result[1].Status);
        Assert.Equal(TransactionType.Debit, result[1].Type);
    }
}