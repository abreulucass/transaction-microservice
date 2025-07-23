using System.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace TransactionMicroservice.Api.Controllers;

public class Transaction
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }

    public string Type { get; set; }        
    public string Status { get; set; }    

    public string Sender { get; set; }               
    public string Recipient { get; set; } 
}


[ApiController]
[Route("/[controller]")]
public class TransactionController
{
    private List<Transaction> transactions =
    [
        new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 250,
            Type = "Credit",
            Status = "completed",
            Sender = "ana.silva",
            Recipient = "lucas.matos",
            Date = DateTime.UtcNow.AddDays(-3)
        },

        new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 120,
            Type = "Debit",
            Status = "pending",
            Sender = "lucas.matos",
            Recipient = "joao.pereira",
            Date = DateTime.UtcNow.AddDays(-1)
        },

        new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 560,
            Type = "Credit",
            Status = "failed",
            Sender = "empresa.xyz",
            Recipient = "lucas.matos",
            Date = DateTime.UtcNow.AddDays(-5)
        },

        new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 50,
            Type = "Debit",
            Status = "COmpleted",
            Sender = "lucas.matos",
            Recipient = "mercado.local",
            Date = DateTime.UtcNow
        },

        new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 1000,
            Type = "Credit",
            Status = "Completed",
            Sender = "cliente.vip",
            Recipient = "lucas.matos",
            Date = DateTime.UtcNow.AddHours(-6)
        }
    ];
    
    [HttpGet]
    public List<Transaction> GetAllTransactions()
    {
        return this.transactions;
    }
}