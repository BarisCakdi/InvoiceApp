using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ItemController : ControllerBase
{
   
    private readonly AppDbContext _context;

    public ItemController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public List<Item> GetPrograms()
    {
        return _context.Items.ToList();
    }

    [HttpPost("/SaveItems")]
    public IActionResult SaveItems([FromBody] dtoSaveItemRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Eksik veya hatalı giriş yaptınız." });
        }
            
        var data = new Item();
            
        data.Total = data.Quantity * data.Price;
            
        if (model.Id is not 0)
        {
            data = _context.Items.Find(model.Id);
            data.Name = model.Name;
            data.Price = model.Price;
            data.InvoiceId = model.Id;
            data.Quantity = model.Quantity;
            _context.Items.Update(data);
        }
        else
        {
            data.Name = model.Name;
            data.Price = model.Price;
            data.Quantity = model.Quantity;
            _context.Items.Add(data);
        }

        _context.SaveChanges();

        return Ok("Ürün başarıyla eklendi.");
    }
    [HttpDelete("{id}")]
    public string DeleteItem(int id)
    {
        try
        {
            var data = _context.Items.Find(id);
            _context.Remove(data);
            _context.SaveChanges();
            return "Başarıyla silindi";
        }
        catch (Exception e)
        {
            return  "Silme işlemi sırıasın bir hata oluştu.\n" + e.Message;
        }
        
        
    }
}