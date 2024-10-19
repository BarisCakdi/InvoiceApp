namespace InvoiceApp.Model;

public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Draft = 3
}


public class Invoice
{
    public int Id { get; set; }

    public string InvoceName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; }

    public int Amount { get; set; }

    public int Total { get; set; }

    public int Quantity { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
    
    public User User { get; set; }
    
    
}