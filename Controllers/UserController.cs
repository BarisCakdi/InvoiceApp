using InvoiceApp.Data;
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
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        [HttpPost]
        public User SaveUser([FromBody] User model)
        {
            if (model != null)
            {
                _context.Users.Update(model);
            }
            else
            {
                _context.Users.Add(model);
            }
            _context.SaveChanges();
            return model;
        }
    }
}
