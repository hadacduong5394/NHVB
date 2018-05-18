using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128), Required, Column(TypeName = "varchar")]
        public string Code { get; set; }

        [MaxLength(512)]
        public string Descreption { get; set; }

        [MaxLength(256)]
        public string CustomerName { get; set; }

        [MaxLength(11)]
        public string CustomerPhone { get; set; }

        [MaxLength(128)]
        public string CustomerEmail { get; set; }

        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int TableId { get; set; }

        [MaxLength(256)]
        public string TableName { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public decimal Sale { get; set; }

        public decimal Payed { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Total { get; set; }// = totalamount - sale

        public bool Status { get; set; } // true = trả đủ, false = trả 1 phần

        public bool IsPayment { get; set; } = false;

        public int ComId { get; set; }
    }
}
