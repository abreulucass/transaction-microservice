
using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateTransaction()
    {
        
        decimal amount = 100m;
        var type = TransactionType.Credit;
        var sender = "UserA";
        var receiver = "UserB";

        // Chama o construtor
        var transaction = new Transaction(amount, type, sender, receiver);

        // Esperado
        Assert.NotNull(transaction.Id);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(type, transaction.Type);
        Assert.Equal(TransactionStatus.Pending, transaction.Status);
        Assert.Equal(sender, transaction.Sender);
        Assert.Equal(receiver, transaction.Receiver);
        Assert.True(transaction.Date <= DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_WithNegativeAmount_ShouldThrowException()
    {
        decimal amount = -50m;
        var type = TransactionType.Credit;
        var sender = "UserA";
        var receiver = "UserB";

        // Chama o construtor e espera a excecao
        var exception = Assert.Throws<ArgumentException>(() =>
            new Transaction(amount, type, sender, receiver));

        Assert.Equal("Amount cannot be negative", exception.Message);
    }
}