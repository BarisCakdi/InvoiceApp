using InvoiceApp.Data;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ItemController : Controller
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

    [HttpPost]
    public IActionResult ItemAdd([FromBody] Item model)
    {
        var data = new Item(); 
        model.Total = model.Quantity * model.Price;
        if (model.Id is not 0)
        {
            data = _context.Items.Find(model.Id);
            data.Name = model.Name;
            data.Quantity = model.Quantity;
            data.Price = model.Price;
            _context.Update(data);
           
        }
        else
        { 
            data.Name = model.Name;
            data.Quantity = model.Quantity;
            data.Price = data.Price;
            _context.Add(data);
        }
        _context.SaveChanges();
        return Ok("Eklendi.");
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