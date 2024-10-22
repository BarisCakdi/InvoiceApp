using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InvoiceApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var users = _context.Clients.ToList();
            return Ok(users);  
        }

        [HttpPost("/SaveClient")]
        public IActionResult SaveClient(dtoSaveClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Eksik veya hatalı giriş yaptınız." });
            }
            
            //denemecanlı
            var data = new Client();

            if (model.Id is not 0)
            {
                data = _context.Clients.Find(model.Id);
                data.Name = model.Name;
                data.Email = model.Email;
                data.Address = model.Address;
                data.City = model.City;
                data.PostCode = model.PostCode;
                data.Country = model.Country;
                _context.Clients.Update(data);
            }
            else
            {
                data.Name = model.Name;
                data.Email = model.Email;
                data.Address = model.Address;
                data.City = model.City;
                data.PostCode = model.PostCode;
                data.Country = model.Country;
                
                _context.Clients.Add(data);
            }

            _context.SaveChanges();

            return Ok("Müşteri Başarıyla eklendi.");
        }
        

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Clients
                .Include(u => u.Invoices)!     // Kullanıcının faturalarını içeri aktar
                .ThenInclude(d => d.Items)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var userWithInvoices = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Address,
                user.City,
                user.PostCode,
                user.Country,
                Invoices = user.Invoices.Select(i => new
                {
                    i.Id,
                    i.InvoiceName,
                    i.CreatedTime,
                    i.PaymentStatus,
                    i.Items
                }).ToList()
            };

            return Ok(userWithInvoices);
        }

        


        [HttpDelete("{id}")]
        public string DeleteClient(int id)
        {
            try
            {
                var client = _context.Clients.Find(id);
                _context.Clients.Remove(client);
                _context.SaveChanges();

                return "Silindi";
            }
            catch (Exception e)
            {
                return "Silinemedi." + e.Message;
            }
        }
    }

}
