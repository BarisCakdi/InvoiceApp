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
}