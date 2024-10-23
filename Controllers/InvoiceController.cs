using System.Net;
using System.Net.Mail;
using InvoiceApp.Data;
using Quartz;
using InvoiceApp.DTOs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        
        
        
        //denemecanlı3

        [HttpGet("/Invoices")]
        public IActionResult GetInvoice(int page = 1, int pageSize = 10)
        {
            var invoices = _context.Invoices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Client)
                .Include(x => x.Items)
                .ToList();

            var invoiceDetail = invoices.Select(invoice => new Invoice
            {
                Id = invoice.Id,
                InvoiceName = invoice.InvoiceName,
                CreatedTime = invoice.CreatedTime,
                PaymentStatus = invoice.PaymentStatus,
                ClientId = invoice.ClientId,
                Client = new Client
                {
                    Id = invoice.Client.Id,
                    Name = invoice.Client.Name,
                    Email = invoice.Client.Email,
                    Address = invoice.Client.Address,
                    PostCode = invoice.Client.PostCode,
                    City = invoice.Client.City,
                    Country = invoice.Client.Country,
                },
                
                Items = invoice.Items.Select(item => new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Quantity * item.Price,
                }).ToList(),
                TotalAmount = invoice.Items.Sum(x => x.Quantity * x.Price),
            }).ToList();

            return Ok(invoiceDetail);
        }

        [HttpPost("/InvoiceDetail/{id}")]
        public IActionResult InvoiceDetail(int id)
        {
            var invoice = _context.Invoices
                .Include(x => x.Client)
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == id);

            if (invoice == null)
            {
                return NotFound("Fatura bulunamadı.");
            }
            
            //deneme

            var invoiceDetail = new Invoice
            {
                Id = id,
                InvoiceName = invoice.InvoiceName,
                CreatedTime = invoice.CreatedTime,
                PaymentStatus = invoice.PaymentStatus,
                ClientId = invoice.ClientId,
                Client = new Client
                {
                    Id = invoice.Client.Id,
                    Name = invoice.Client.Name,
                    Email = invoice.Client.Email,
                    Address = invoice.Client.Address,
                    PostCode = invoice.Client.PostCode,
                    City = invoice.Client.City,
                    Country = invoice.Client.Country,
                },
                Items = invoice.Items.Select(x => new Item
                {
                    Id = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Total = x.Quantity * x.Price,
                }).ToList(),
                TotalAmount = invoice.Items.Sum(x => x.Quantity * x.Price),
            };

            return Ok(invoiceDetail);
        }

        [HttpPost]
        public IActionResult SaveInvoice([FromBody] dtoSaveInvoiceRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Eksik veya hatalı giriş yaptınız." });
            }

            if (model.Id == 0)
            {
                var invoice = new Invoice()
                {
                    InvoiceName = GenerateInvoiceName(),
                    CreatedTime = model.CreatedTime,
                    PaymentStatus = PaymentStatus.Pending,
                    Description = model.Description,
                    ClientId = model.ClientId,
                    PaymentDue = CalculatePaymentDue(model.CreatedTime, model.PaymentTerm), 
                    
                    Items = model.Items.Select(x => new Item
                    {
                        Name = x.Name,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Total = x.Price * x.Quantity,
                    }).ToList()
                };

                if (model.PaymentStatus == PaymentStatus.Pending)
                {
                    var client = new SmtpClient("smtp.eu.mailgun.org", 587)
                    {
                        Credentials = new NetworkCredential("postmaster@mesaj.denemeinvoice.com.tr",
                            "86292b638e47e76d91b20ba5c630675c-fe9cf0a8-f63be79c"),
            
                        EnableSsl = true    
                    };

                    var mailMessage = new MailMessage()  //paranteze gerek yok
                    {
                        From = new MailAddress("postmaster@mesaj.denemeinvoice.com.tr","Deneme Invoice"), // postmaster yerine istediğimiz olur 
                        Subject = $"denemeınvoice.com.tr'den mesaj var",
                        Body = "Faturanız başarıyla oluşturuldu." ,
                        IsBodyHtml = true

                    };
        
        
                    // mailMessage.To.Add("cimebura@hotmail.com");

                    mailMessage.To.Add(new MailAddress("denememail@gmail.com","Deneme Email"));
        
                    client.Send(mailMessage);

                   return Ok(new {message = "Mail Gönderildi"});
                }

                _context.Invoices.Add(invoice);
                _context.SaveChanges();

                return Ok(new { message = "Fatura başarıyla kaydedildi." });
            }
            else
            {
                var invoice = _context.Invoices
                    .Include(x => x.Items)
                    .FirstOrDefault(x => x.Id == model.Id);

                if (invoice == null)
                {
                    return NotFound("Fatura bulunamadı.");
                }

                invoice.CreatedTime = model.CreatedTime;
                invoice.Description = model.Description;
                // invoice.ClientId = model.ClientId;
                invoice.PaymentDue = model.PaymentDue;

                // Ödeme durumu güncellemeleri
                if (model.PaymentStatus == PaymentStatus.Paid)
                {
                    invoice.PaymentStatus = PaymentStatus.Paid; 
                    
                    var client = new SmtpClient("smtp.eu.mailgun.org", 587)
                    {
                        Credentials = new NetworkCredential("postmaster@mesaj.denemeinvoice.com.tr",
                            "86292b638e47e76d91b20ba5c630675c-fe9cf0a8-f63be79c"),
            
                        EnableSsl = true    
                    };

                    var mailMessage = new MailMessage()  //paranteze gerek yok
                    {
                        From = new MailAddress("postmaster@mesaj.denemeinvoice.com.tr","Deneme Invoice"), // postmaster yerine istediğimiz olur 
                        Subject = $"denemeınvoice.com.tr'den mesaj var",
                        Body = "Faturanız başarıyla ödendi." ,
                        IsBodyHtml = true

                    };
        
        
                    // mailMessage.To.Add("cimebura@hotmail.com");

                    mailMessage.To.Add(new MailAddress("denememail@gmail.com","Deneme Email"));
        
                    client.Send(mailMessage);

                    return Ok(new {message = "Mail Gönderildi"});
                }
                else if (DateTime.Now > model.PaymentDue)
                {
                    invoice.PaymentStatus = PaymentStatus.Draft; 
                    var client = new SmtpClient("smtp.eu.mailgun.org", 587)
                    {
                        Credentials = new NetworkCredential("postmaster@mesaj.denemeinvoice.com.tr",
                            "86292b638e47e76d91b20ba5c630675c-fe9cf0a8-f63be79c"),
            
                        EnableSsl = true    
                    };

                    var mailMessage = new MailMessage()  //paranteze gerek yok
                    {
                        From = new MailAddress("postmaster@mesaj.denemeinvoice.com.tr","Deneme Invoice"), // postmaster yerine istediğimiz olur 
                        Subject = $"denemeınvoice.com.tr'den mesaj var",
                        Body = "Faturanız başarıyla ödenmedi." ,
                        IsBodyHtml = true

                    };
        
        
                    // mailMessage.To.Add("cimebura@hotmail.com");

                    mailMessage.To.Add(new MailAddress("denememail@gmail.com","Deneme Email"));
        
                    client.Send(mailMessage);

                    return Ok(new {message = "Mail Gönderildi"});
                }
                else
                {
                    invoice.PaymentStatus = PaymentStatus.Pending; 
                    var client = new SmtpClient("smtp.eu.mailgun.org", 587)
                    {
                        Credentials = new NetworkCredential("postmaster@mesaj.denemeinvoice.com.tr",
                            "86292b638e47e76d91b20ba5c630675c-fe9cf0a8-f63be79c"),
            
                        EnableSsl = true    
                    };

                    var mailMessage = new MailMessage()  //paranteze gerek yok
                    {
                        From = new MailAddress("postmaster@mesaj.denemeinvoice.com.tr","Deneme Invoice"), // postmaster yerine istediğimiz olur 
                        Subject = $"denemeınvoice.com.tr'den mesaj var",
                        Body = "Faturanız askıya alındı." ,
                        IsBodyHtml = true

                    };
        
        
                    // mailMessage.To.Add("cimebura@hotmail.com");

                    mailMessage.To.Add(new MailAddress("denememail@gmail.com","Deneme Email"));
        
                    client.Send(mailMessage);

                    // return Ok(new {message = "Mail Gönderildi"});
                }

                // Item'ları güncelleme
                invoice.Items.Clear();
                invoice.Items = model.Items.Select(x => new Item
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Total = x.Price * x.Quantity,
                }).ToList();

                _context.Invoices.Update(invoice);
                _context.SaveChanges();

                return Ok(new { message = "Fatura başarıyla güncellendi." });
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault(x => x.Id == id);

            if (invoice == null)
            {
                return NotFound("Fatura bulunamadı.");
            }

            _context.Invoices.Remove(invoice);
            _context.SaveChanges();

            return Ok("Fatura başarıyla silindi.");
        }

        [HttpGet("/Invoices/Status/{status}")]
        public IActionResult GetInvoicesByStatus(Status status)
        {
            var invoices = _context.Invoices
                .Include(x => x.Client)
                .Include(x => x.Items)
                .Where(x => x.PaymentStatus == (PaymentStatus)status)
                .ToList();

            var invoiceDtos = invoices.Select(invoice => new Invoice
            {
                Id = invoice.Id,
                InvoiceName = invoice.InvoiceName,
                CreatedTime = invoice.CreatedTime,
                PaymentStatus = invoice.PaymentStatus,
                ClientId = invoice.ClientId,
                Client = new Client
                {
                    Id = invoice.Client.Id,
                    Name = invoice.Client.Name,
                    Email = invoice.Client.Email,
                    Address = invoice.Client.Address,
                    PostCode = invoice.Client.PostCode,
                    City = invoice.Client.City,
                    Country = invoice.Client.Country
                },
                Items = invoice.Items.Select(item => new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Quantity * item.Price,
                }).ToList()
            }).ToList();

            return Ok(invoiceDtos);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateInvoiceName()
        {
            Random random = new Random();

            char firstLetter = (char)random.Next('A', 'Z' + 1);
            char secondLetter = (char)random.Next('A', 'Z' + 1);
            int number = random.Next(1000, 9999);
            var invoiceName =
                _context.Invoices.FirstOrDefault(x => x.InvoiceName == $"{firstLetter}{secondLetter}{number}");
            if (invoiceName != null)
            {
                char firstLetterAgain = (char)random.Next('A', 'Z' + 1);
                char secondLetterAgain = (char)random.Next('A', 'Z' + 1);
                int numberAgain = random.Next(1000, 9999);

                return $"{firstLetter}{secondLetter}{number}";

            }
            else
            {
                return $"{firstLetter}{secondLetter}{number}";

            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public DateTime CalculatePaymentDue(DateTime createdTime, PaymentTerm paymentTerm)
        {
            if (paymentTerm == PaymentTerm.ErtesiGün)
            {
                return createdTime.AddDays(1);
            }
            else if (paymentTerm == PaymentTerm.Sonraki7Gün)
            {
                return createdTime.AddDays(7);

            }
            else if (paymentTerm == PaymentTerm.Sonraki14Gün)
            {
                return createdTime.AddDays(14);
            }
            else
            {
                return createdTime.AddDays(30);
            }
        }
        





    }
}