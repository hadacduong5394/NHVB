using cvmk.context.domain.Products;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IProductMaterialService: IBaseService<ProductMaterials, int>
    {
        IList<ProductMaterials> GetbyProductId(int productId);
    }
}
