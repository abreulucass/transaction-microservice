using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Application.DTOs;

public class CreateTransactionDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Receiver { get; set; }
    public string Sender { get; set; }
}