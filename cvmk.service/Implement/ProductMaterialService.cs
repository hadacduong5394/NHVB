using cvmk.context.domain.Products;
using cvmk.service.Interface;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class ProductMaterialService: BaseService<ProductMaterials, int>, IProductMaterialService
    {
        public ProductMaterialService(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IList<ProductMaterials> GetbyProductId(int productId)
        {
            return Query.Where(n => n.ProductId == productId).ToList();
        }
    }
}
