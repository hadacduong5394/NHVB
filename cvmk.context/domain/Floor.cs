using hdcontext.AdminDomain.Abtracttions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("Floors")]
    public class Floor : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Descreption { get; set; }

        public int ComId { get; set; }

        public bool VIP { get; set; }
    }
}