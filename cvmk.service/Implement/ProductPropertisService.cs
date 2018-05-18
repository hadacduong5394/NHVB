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
    public class ProductPropertisService: BaseService<ProductPropertis, int>, IProductPropertisService
    {
        public ProductPropertisService(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
