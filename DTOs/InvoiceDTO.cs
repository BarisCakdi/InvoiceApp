using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.DTOs;

public class InvoiceDTO
{
    public int Id { get; set; }
    public string InvoiceName { get; set; }
    public DateTime CreatedTime { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
    public List<ItemDTO> Items { get; set; }
}
