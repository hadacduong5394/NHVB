using hdcontext.AdminDomain.Domain;
using hddata.RepositoryPattern;
using System.Collections.Generic;

namespace cvmk.service.Interface
{
    public interface ILeftMenuService : IBaseService<LeftMenu, int>
    {
        bool Create(LeftMenu entity, out string message);

        bool Update(LeftMenu entity, out string message);

        bool Delete(int id, out string message);

        IList<LeftMenu> GetParents();

        IList<LeftMenu> GetChilds(int parentId);

        IList<LeftMenu> GetbyFilter(int parentId, int currentPage, int pageSize, out int total);
    }
}