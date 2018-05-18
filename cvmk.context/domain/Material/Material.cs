using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain.Material
{
    [Table("Materials")]
    public class Material: AbMaterial
    {
        public string Images { get; set; }
    }
}
