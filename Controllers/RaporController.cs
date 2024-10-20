using InvoiceApp.Data;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GenerateSalesReport()
        {
            
            var invoices = _context.Invoices
                                   .Include(i => i.Items)
                                   .Include(i => i.User)
                                   .ToList();

            if (invoices == null || invoices.Count == 0)
            {
                return NotFound("Fatura Bulunamadı.");
            }

            
            var report = new
            {
                TotalSales = invoices.Sum(inv => inv.Items.Sum(item => item.Total)),
                TotalInvoices = invoices.Count,
                TotalItemsSold = invoices.Sum(inv => inv.Items.Sum(item => item.Quantity)),
                UserReports = invoices.GroupBy(inv => inv.User)
                                      .Select(grp => new
                                      {
                                          UserName = grp.Key.Name,
                                          UserEmail = grp.Key.Email,
                                          TotalInvoices = grp.Count(),
                                          TotalUserSales = grp.Sum(inv => inv.Items.Sum(item => item.Total)),
                                          TotalUserItemsSold = grp.Sum(inv => inv.Items.Sum(item => item.Quantity))
                                      }).ToList()
            };

            return Ok(report);
        }
    }
}
