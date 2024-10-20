using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);  
        }


        [HttpGet("User/{id}")]
        public IActionResult GetUser(int id)
        {
            var client = _context.Users.FirstOrDefault(x => x.Id == id);

            if (client == null)
            {
                return BadRequest("Kullanıcı bulunamadı");
            }

            return Ok(client);
        }



        [HttpDelete("{id}")]
        public string DeleteClient(int id)
        {
            try
            {
                var client = _context.Users.Find(id);
                _context.Users.Remove(client);
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
