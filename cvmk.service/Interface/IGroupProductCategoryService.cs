using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IGroupProductCategoryService : IBaseService<GroupProductCategory, int>
    {
        bool Create(GroupProductCategory entity, out string message);

        IList<GroupProductCategory> GetbyFilter(int com_id, string name, int currentPage, int pageSize, out int total);

        bool Update(GroupProductCategory entity, out string message);

        bool Delete(int id, out string message);
    }
}
