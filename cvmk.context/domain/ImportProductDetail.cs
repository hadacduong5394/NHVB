using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("ImportProductDetails")]
    public class ImportProductDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int ImportProductId { get; set; }
        public int MaterialId { get; set; }
        [Required]
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Descreption { get; set; }
    }
}