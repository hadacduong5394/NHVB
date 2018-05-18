using hdcontext.AdminDomain.Abtracttions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("ImportProducts")]
    public class ImportProduct : Auditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(128), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }

        [Required]
        public string UserId { get; set; }

        public int VAT { get; set; }

        public decimal TotalAmount { get; set; }

        [MaxLength(256)]
        public string SuppierCode { get; set; }

        [MaxLength(256)]
        public string SupplierName { get; set; }

        [MaxLength(20)]
        public string SupplierTaxcode { get; set; }

        [MaxLength(128)]
        public string EmailSupplier { get; set; }

        [MaxLength(256)]
        public string AddressSupplier { get; set; }

        [MaxLength(11)]
        public string PhoneSupplier { get; set; }

        public DateTime ImportDate { get; set; }

        [MaxLength(256)]
        public string Descreption { get; set; }

        public int ComId { get; set; }
    }
}