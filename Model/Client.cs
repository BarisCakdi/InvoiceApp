using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public int? PostCode { get; set; }

        public string? Country { get; set; }

        // Lazy Loading için 'virtual' eklenebilir
        public virtual ICollection<Invoice>? Invoices { get; set; } = new List<Invoice>();
    }

    

}
