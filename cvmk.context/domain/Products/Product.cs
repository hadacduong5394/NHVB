using hdcontext.AdminDomain.Abtracttions;
using hdcore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain.Products
{
    [Table("Products")]
    public class Product : Auditable, IProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ComId { get; set; }

        [MaxLength(128), Column(TypeName = "varchar"), Required]
        public string BarCode { get; set; }

        [MaxLength(128), Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Descreption { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string Images { get; set; }

        public string Content { get; set; }

        public int Quantity { get; set; }

        public decimal RootPrice { get; set; }

        public decimal Price { get; set; }

        [MaxLength(128)]
        public string Unit { get; set; }

        public int TypeId { get; set; }

        public int GroupId { get; set; }

        public bool VIP { get; set; }
    }
}