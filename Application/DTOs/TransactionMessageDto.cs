using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Application.DTOs;

public class TransactionMessageDto
{
    public string Id { get; set; }
    public decimal Amount { get; set; }
    
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }

    public string Sender { get; set; }
    public string Receiver { get; set; }
    public DateTime Date { get; set; }
}