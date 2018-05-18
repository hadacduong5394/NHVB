using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface ITypeProductCategoryService: IBaseService<TypeProductCategory, int>
    {
        bool Create(TypeProductCategory entity, out string message);

        IList<TypeProductCategory> GetbyFilter(int com_id, string name, int currentPage, int pageSize, out int total);

        bool Update(TypeProductCategory entity, out string message);

        bool Delete(int id, out string message);
    }
}
