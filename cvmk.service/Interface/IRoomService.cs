using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IRoomService: IBaseService<Room, int>
    {
        IList<Room> GetbyFilter(int com_id, int floorId, string name, int currentPage, int pageSize, out int total);

        bool Create(Room room, out string message);

        bool Update(Room room, out string message);

        bool Delete(int id, out string message);

        IList<Room> GetbyFloorId(int floorId);

        IList<Room> GetTableIsNotWorking(int com_id, int floorId);

        IList<Room> GetTableWorking(int com_id, int momentTableId, int floorId);
    }
}
