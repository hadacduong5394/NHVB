using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain.Products
{
    [Table("UnitProducts")]
    public class UnitProduct : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }
    }
}