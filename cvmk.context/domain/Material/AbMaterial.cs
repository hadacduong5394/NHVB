using hdcontext.AdminDomain.Abtracttions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain.Material
{
    public abstract class AbMaterial: Auditable, IMaterial
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }

        [MaxLength(128), Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Descreption { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }

        public int RootPrice { get; set; }

        [MaxLength(128)]
        public string Unit { get; set; }

        public int ComId { get; set; }
    }
}
