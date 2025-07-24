
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
    private Mock<IMongoClient> _mockClient;
    private Mock<IMongoDatabase> _mockDatabase;
    private Mock<IMongoCollection<Transaction>> _mockCollection;
    private IOptions<MongoDbSettings> _mockSettings;
    
    public DbTransactionRepositoryTests()
    {
        _mockClient = new Mock<IMongoClient>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockCollection = new Mock<IMongoCollection<Transaction>>();
        _mockSettings = Options.Create(new MongoDbSettings
        {
            DatabaseName = "TestDb",
            CollectionName = "Transactions"
        });

        _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(_mockDatabase.Object);

        _mockDatabase.Setup(db => db.GetCollection<Transaction>(It.IsAny<string>(), null))
            .Returns(_mockCollection.Object);
    }

    private IAsyncCursor<Transaction> CreateAsyncCursor(List<Transaction> list)
    {
        var mockCursor = new Mock<IAsyncCursor<Transaction>>();
        mockCursor.Setup(_ => _.Current).Returns(list);
        mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        return mockCursor.Object;
    }
    
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnEmptyList_WhenNoTransactionsExist()
    {
        var cursor = CreateAsyncCursor(new List<Transaction>());
        _mockCollection
            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Transaction>>(),
                It.IsAny<FindOptions<Transaction, Transaction>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursor);

        var repository = new DbTransacationRepository(_mockClient.Object, _mockSettings);

        var result = await repository.GetAllTransactionsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCallInsertOneAsync()
    {
        var transaction = new Transaction(100m, TransactionType.Credit, "Teste", "Banco X");

        _mockCollection
            .Setup(c => c.InsertOneAsync(transaction, null, default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var repository = new DbTransacationRepository(_mockClient.Object, _mockSettings);

        await repository.CreateAsync(transaction);

        _mockCollection.Verify(c => c.InsertOneAsync(transaction, null, default), Times.Once);
    }
    
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnMockedTransactions()
    {
        var mockedTransactions = new List<Transaction>
        {
            new Transaction(100m, TransactionType.Credit, "Lucas", "Maria"),
            new Transaction(50m, TransactionType.Debit, "Mario", "Patricia"),
        };

        var cursor = CreateAsyncCursor(mockedTransactions);

        _mockCollection
            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Transaction>>(),
                It.IsAny<FindOptions<Transaction, Transaction>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursor);

        var repository = new DbTransacationRepository(_mockClient.Object, _mockSettings);

        var result = await repository.GetAllTransactionsAsync();

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