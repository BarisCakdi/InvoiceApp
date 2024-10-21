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

        [HttpPost]
        public IActionResult SaveClient([FromBody] dtoSaveClientRequest model)
        {
            if (model == null)
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            if (ModelState.IsValid)
            {
                var data = new Client
                {
                    Name = model.Name,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    PostCode = model.PostCode,
                    Country = model.Country
                };

                if (data.Id == 0)
                {
                    _context.Clients.Add(data);
                    _context.SaveChanges();
                    return Ok(new { message = "Kullanıcı başarıyla eklendi." });
                }
            }
            return BadRequest(new { message = "Geçersiz girişler mevcut" });
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Clients
                .Include(u => u.Invoices) // Kullanıcının faturalarını içeri aktar
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



        [HttpPut]
        public IActionResult UpdateClient([FromBody] dtoUpdateClientRequest model)
        {
            if (model == null || model.Id == 0)
            {
                return BadRequest("Geçersiz kullanıcı bilgileri.");
            }

            var data = _context.Clients.Find(model.Id);
            if (data == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            if (ModelState.IsValid)
            {
                data.Name = model.Name;
                data.Email = model.Email;
                data.Address = model.Address;
                data.City = model.City;
                data.PostCode = model.PostCode;
                data.Country = model.Country;

                _context.Clients.Update(data);
                _context.SaveChanges();
                return Ok("Kullanıcı başarıyla güncellendi.");
            }

            return BadRequest("Geçersiz girişler mevcut.");
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
