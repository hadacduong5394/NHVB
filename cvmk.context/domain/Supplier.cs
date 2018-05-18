using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("Suppliers")]
    public class Supplier : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }

        [MaxLength(128), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(500)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Logo { get; set; }

        [MaxLength(500)]
        public string Descreption { get; set; }

        [MaxLength(50), Column(TypeName = "varchar")]
        public string TaxCode { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        public int ComId { get; set; }
    }
}