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

    public Double Price { get; set; }

    public Double Total { get; set; }

    public int InvoiceId { get; set; }
    
    [ForeignKey("InvoiceId")]
    public Invoice Invoice { get; set; }
    

}
