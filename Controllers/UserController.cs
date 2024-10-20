﻿using InvoiceApp.Data;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InvoiceApp.DTOs;

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

        [HttpPost]
        public IActionResult SaveClient([FromBody]dtoSaveClientRequest model)
        {
            var data = new User();
            if (data == null)
            {
                return BadRequest("Geçersiz kullanıcı bilgisi gönderildi.");
            }

            if (ModelState.IsValid)  
            {
                if (data.Id == 0)
                {
                    _context.Users.Add(data);
                    _context.SaveChanges();
                    return Ok(new { message = "Kullanıcı başarıyla eklendi." }); 
                }
            }

            return BadRequest(new { message = "Geçersiz girişler mevcut."});  
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
