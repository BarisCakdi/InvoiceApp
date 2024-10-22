using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.DTOs;

public class dtoSaveItemRequest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

   

}