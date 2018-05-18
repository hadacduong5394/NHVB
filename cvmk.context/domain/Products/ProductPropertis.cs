using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain.Products
{
    [Table("ProductPropertis")]
    public class ProductPropertis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar"), Required, MaxLength(50)]
        public string Key { get; set; }

        public int ProductId { get; set; }

        [MaxLength(128), Required]
        public string Value { get; set; }
    }
}