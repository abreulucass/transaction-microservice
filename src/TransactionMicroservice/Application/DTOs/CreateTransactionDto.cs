using System.ComponentModel.DataAnnotations;
using TransactionMicroservice.Domain.Enums;

namespace TransactionMicroservice.Application.DTOs;

public class CreateTransactionDto
{
    [Required(ErrorMessage = "O Valor da transação é obrigatorio")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "O Tipo da transação é obrigatorio")]
    [EnumDataType(typeof(TransactionType), ErrorMessage = "Os tipos permitidos são Credit e Debit")]
    public TransactionType Type { get; set; }
    
    [Required(ErrorMessage = "O nome da pessoa/empresa que vai receber a transação é obrigatorio")]
    [MinLength(5, ErrorMessage = "O nome deve ter no mínimo 5 caracteres.")]
    [MaxLength(20, ErrorMessage = "O nome deve ter no máximo 20 caracteres.")]
    public string Receiver { get; set; }
    
    [Required(ErrorMessage = "O nome da pessoa/empresa que esta enviando a transação é obrigatorio")]
    [MinLength(5, ErrorMessage = "O nome deve ter no mínimo 5 caracteres.")]
    [MaxLength(20, ErrorMessage = "O nome deve ter no máximo 20 caracteres.")]
    public string Sender { get; set; }
}