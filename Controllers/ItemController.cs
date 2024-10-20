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
        if(model == null)
        {
            return BadRequest("Eksik bilgi girişi!");
        }
        _context.Items.Add(model);
        _context.SaveChanges();
        return Ok(new { message = "Başarıyla eklendi" });
    }
}