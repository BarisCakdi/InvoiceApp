using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Draft = 3
    }

    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string InvoiceName { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedTime { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        // Lazy Loading için 'virtual' eklenebilir
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
