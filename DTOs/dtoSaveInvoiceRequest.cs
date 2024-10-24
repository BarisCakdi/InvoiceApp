using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InvoiceApp.DTOs;

public class dtoSaveInvoiceRequest
{
    public int Id { get; set; }

   
    public string InvoiceName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; }
    public ICollection<dtoSaveItemRequest> Items { get; set; } 

    public PaymentStatus PaymentStatus { get; set; }
  
    public PaymentTerm PaymentTerm { get; set; }
    public DateTime PaymentDue { get; set; }
    public int ClientId { get; set; } 
}