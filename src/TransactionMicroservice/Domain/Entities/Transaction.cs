using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Domain.Entities;

public class Transaction
{
    public string Id { get; private set; } 
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    
    public TransactionType Type { get; private set; } 
    public TransactionStatus Status { get; private set; }
    
    public string Sender { get; private set; }              
    public string Receiver { get; private set; }
    
    
    public Transaction(decimal amount, TransactionType type, string sender, string receiver)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative");

        Id = Guid.NewGuid().ToString();
        Date = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
        Amount = amount;
        Type = type;
        Sender = sender;
        Receiver = receiver;
    }

    private Transaction()
    {
        
    }
}