using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("UnitTag")]
    public class UnitTag
    {
        [Key]
        [Column(TypeName = "varchar"), MaxLength(70)]
        public string Id { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }
    }
}