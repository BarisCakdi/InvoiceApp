using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model;

public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Draft = 3
}

public enum PaymentTerm
{
    ErtesiGün,
    Sonraki7Gün,
    Sonraki14Gün,
    Sonraki30Gün
}

public enum Status
{
    Beklemede,
    Ödendi,
    Taslak
}

public class Invoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string InvoiceName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; }
    

    public ICollection<Item> Items { get; set; }
    
    
    public DateTime PaymentDue { get; set; }

    public Double TotalAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    
    // Bir faturanın birden fazla Item'ı olabilir
    
     public int ClientId { get; set; }
    public Client? Client { get; set; }
}
