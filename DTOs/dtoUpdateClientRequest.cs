namespace InvoiceApp.DTOs;

public class dtoUpdateClientRequest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public int PostCode { get; set; }

    public string Country { get; set; }
}