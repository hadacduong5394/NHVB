using hdcontext.AdminDomain.Domain;
using hddata.RepositoryPattern;
using System.Collections.Generic;

namespace cvmk.service.Interface
{
    public interface ITopMenuService : IBaseService<TopMenu, int>
    {
        IList<TopMenu> GetChilds(int parentId);

        IList<TopMenu> GetParents();

        IList<TopMenu> GetbyFilter(int parentId, int currentPage, int pageSize, out int total);

        bool Create(TopMenu entity, out string message);

        bool Update(TopMenu entity, out string message);

        bool Delete(int id, out string message);
    }
}