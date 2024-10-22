using InvoiceApp.Data;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet]
    public IActionResult GetCompanys()
    {
        var company = _context.Companys.ToList();
        return Ok(company);  
    }
    
    [HttpPost]
    public Company SaveCompany([FromBody] dtoSaveCompanyRequest model)
    {
        var data = new Company(); 
        if (model.Id is not 0)
        {
            data = _context.Companys.Find(model.Id);
            data.CompanyCountry = model.CompanyCountry;
            data.CompanyAddress = model.CompanyAddress;
            data.CompanyPostCode = model.CompanyPostCode;
            _context.Companys.Update(data);
           
        }
        else
        { 
            data.CompanyCountry = model.CompanyCountry;
            data.CompanyAddress = model.CompanyAddress;
            data.CompanyPostCode = model.CompanyPostCode;
            _context.Companys.Add(data);
        }
        _context.SaveChanges();
        return _context.Companys.Find(model.Id);
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