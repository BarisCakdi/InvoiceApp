using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.DTOs;

public class dtoAddItemRequest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int Total { get; set; }

}