using hdcontext.AdminDomain.Abtracttions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain
{
    [Table("Customers")]
    public class Customer: Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Type { get; set; } = 1; //1 Cá nhân, 2 công ty

        [MaxLength(128), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Company { get; set; }

        [MaxLength(128), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }

        public DateTime? BirthDay { get; set; }

        [MaxLength(500)]
        public string TaxCode { get; set; }

        [MaxLength(500)]
        public string Avatar { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(500)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(500)]
        public string Descreption { get; set; }

        public int ComId { get; set; }
    }
}
