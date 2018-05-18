using cvmk.context.domain.Material;
using cvmk.context.domain.Products;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IProductService: IBaseService<Product, int>
    {
        IList<Product> GetbyFilter(int com_id, string name, string code, int currentPage, int pageSize, out int total);

        bool Create(Product product, IList<ProductMaterials> materials, IList<ProductPropertis> lstProp, out string message);

        bool Update(Product product, IList<ProductMaterials> materials, IList<ProductPropertis> lstProp, out string message);

        bool Delete(int id,out string message);

        IList<Product> GetMenuProductFilter(int com_id, string code_name, int floorId, int groupId, int typeId, int currentPage, int pageSize, out int total);
    }
}
