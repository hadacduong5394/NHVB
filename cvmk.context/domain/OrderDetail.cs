using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [MaxLength(128), Required, Column(TypeName = "varchar")]
        public string ProductCode { get; set; }

        [MaxLength(256)]
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal RootPrice { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; } //Amount = Quantity * Price
    }
}
