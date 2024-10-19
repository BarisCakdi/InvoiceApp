using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int Total { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; }
}
