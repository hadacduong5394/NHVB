using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IFloorService: IBaseService<Floor, int>
    {
        IList<Floor> GetbyFilter(int comId, string name, int currentPage, int pageSize, out int total);

        bool Create(Floor floor, out string message);

        bool Update(Floor floor, out string message);

        bool Delete(int id, out string message);
    }
}
