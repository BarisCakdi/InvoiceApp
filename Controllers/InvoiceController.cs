using InvoiceApp.Data;
using InvoiceApp.Model;
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
    
    public List<Invoice> GetInvoices()
    {
        var ınvoice = _context.Invoices.ToList();
        return ınvoice;
    }

    // [HttpGet]
    // [Route("[id]")]
    // yukarıdaki iki satır yerine bunları birleştirip tek satırda yazabiliyoruz.
    [HttpGet("{id}")]
    public Invoice GetInvoice(int id)
    {
        var lesson = _context.Invoices.Find(id);
        if (lesson is null) 
            return new Invoice(); // NotFound()
        return lesson; // Ok(lesson)
    }

    [HttpPost]
    public IActionResult SaveInvoice([FromBody] Invoice model)
    {
        
            _context.Invoices.Add(model); 
            _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoice), new { id = model.Id }, model);
       
    }
    public IActionResult UpdateInvoice([FromBody] Invoice model)
    {
        model = _context.Invoices.Find(model.Id);
        model.InvoiceName = model.InvoiceName;
        model.Description = model.Description;
        model.PaymentStatus = model.PaymentStatus;
        model.User = model.User;
        _context.Invoices.Update(model); 
        _context.SaveChangesAsync();
        return Ok("Başarılıyla kaydedildi.");
       
    }


    [HttpDelete("{id}")]
    public string DeleteInvoice(int id)
    {
        try
        {
            var data = _context.Invoices.Find(id);
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