using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model;

public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Draft = 3
}

public class Invoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string InvoiceName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    // Bir faturanın birden fazla Item'ı olabilir
    public List<Item> Items { get; set; }
    
}
