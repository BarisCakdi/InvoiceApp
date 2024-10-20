using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.DTOs;

public class dtoUpdateInvoiceRequest
{
    public int Id { get; set; }

    public string InvoiceName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

}