using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetInvoices()
        {
            var ınvoice = _context.Invoices.ToList();
            return Ok(ınvoice);
        }

        [HttpGet("{id}")]
        public Invoice GetInvoice(int id)
        {
            var lesson = _context.Invoices.Find(id);
            if (lesson is null)
                return new Invoice();
            return lesson;
        }

        [HttpPost]
        public IActionResult SaveInvoice([FromBody]dtoSaveInvoiceRequest model)
        {
            var data = new Invoice();

            _context.Invoices.Add(data);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetInvoice), new { id = data.Id }, model);

        }
        [HttpPut]
        public IActionResult UpdateInvoice([FromBody] dtoUpdateInvoiceRequest model)
        {
            var data = new Invoice();
            
            data = _context.Invoices.Find(model.Id);
            data.InvoiceName = data.InvoiceName;
            data.Description = data.Description;
            data.PaymentStatus = data.PaymentStatus;
            data.User = data.User;
            _context.Invoices.Update(data);
            _context.SaveChanges();
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
                return "Silme işlemi sırıasın bir hata oluştu." + e.Message;
            }

        }

        //[HttpPost]
        //public IActionResult MailPost()
        //{

        //}
    }
}