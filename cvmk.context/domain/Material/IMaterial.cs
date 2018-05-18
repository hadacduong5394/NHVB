using hdcontext.AdminDomain.Abtracttions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.context.domain.Material
{
    public interface IMaterial: IAuditable
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Descreption { get; set; }
        string Image { get; set; }
        int Quantity { get; set; }
        int RootPrice { get; set; }
        string Unit { get; set; }
        int ComId { get; set; }
    }
}
