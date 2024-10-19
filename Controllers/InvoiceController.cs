using InvoiceApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InvoiceController : Controller
{
    private readonly AppDbContext _context;
    public InvoiceController(AppDbContext context)
    {
        _context = context;
    }
}