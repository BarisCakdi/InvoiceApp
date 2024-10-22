using InvoiceApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.DTOs;

<<<<<<<< HEAD:DTOs/ItemDTO.cs
public class ItemDTO
========
public class dtoSaveItemRequest
>>>>>>>> Mehmet:DTOs/dtoSaveItemRequest.cs
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
<<<<<<<< HEAD:DTOs/ItemDTO.cs
    public decimal Price { get; set; }
    public decimal Total { get; set; }
========

    public int Price { get; set; }

   
>>>>>>>> Mehmet:DTOs/dtoSaveItemRequest.cs

}