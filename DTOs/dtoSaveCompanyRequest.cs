using InvoiceApp.Model;

namespace InvoiceApp.DTOs;

public class dtoSaveCompanyRequest
{
    public int Id { get; set; }
        
    public string CompanyAddress { get; set; }
        
    public int CompanyPostCode { get; set; }
        
    public string CompanyCountry { get; set; }
   
    
    
}