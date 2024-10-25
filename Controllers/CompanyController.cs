using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CompanyController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompanyController(AppDbContext context)
    {
        _context = context;
    }
    //denemecanlı2
    [HttpGet]
    public IActionResult GetCompany()
    {
        var users = _context.Companys.ToList();
        return Ok(users);  
    }

    [HttpPost("/SaveCompany")]
    public IActionResult SaveCompany(dtoSaveCompanyRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Eksik veya hatalı giriş yaptınız." });
        }
        var data = new Company();

        if (model.Id is not 0)
        {
            
            data = _context.Companys.Find(model.Id);
            data.CompanyAddress = model.CompanyAddress;
            data.CompanyCountry = model.CompanyCountry;
            data.CompanyPostCode = model.CompanyPostCode;
            _context.Companys.Update(data);
        }
        else
        {
            data.CompanyAddress = model.CompanyAddress;
            data.CompanyCountry = model.CompanyCountry;
            data.CompanyPostCode = model.CompanyPostCode;
            
                
            _context.Companys.Add(data);
                
        }

        _context.SaveChanges();

        return Ok("Şirket Başarıyla eklendi.");
    }
    

    [HttpDelete("{id}")]
    public string DeleteCompany(int id)
    {
        try
        {
            var company = _context.Companys.Find(id);
            _context.Companys.Remove(company);
            _context.SaveChanges();

            return "Silindi";
        }
        catch (Exception e)
        {
            return "Silinemedi." + e.Message;
        }
    }
}