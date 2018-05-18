using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class RoomService : BaseService<Room, int>, IRoomService
    {
        private readonly IErrorService log;
        public RoomService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Room room, out string message)
        {
            try
            {
                room.CreateBy = CurrentUser.Instance.User.UserName;
                room.CreateDate = DateTime.Now;
                room.ComId = CurrentUser.Instance.User.ComId;
                CreateNew(room);
                CommitChange();
                message = hdcore.Utils.TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            try
            {
                Delete(id);
                CommitChange();
                message = hdcore.Utils.TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public IList<Room> GetbyFilter(int com_id, int floorId, string name, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (floorId != -1)
            {
                query = query.Where(n => n.FloorId == floorId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }

            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<Room> GetbyFloorId(int floorId)
        {
            return GetMulti(n => n.FloorId == floorId && n.Status == true).ToList();
        }

        public IList<Room> GetTableIsNotWorking(int com_id, int floorId)
        {
            var floorSrv = IoC.Resolve<IFloorService>();
            var floor = floorSrv.GetbyKey(floorId);
            var tables = (from a in Query
                          join b in IoC.Resolve<IFloorService>().Query on a.FloorId equals b.Id
                          where a.Status == true && b.Status == true &&
                          a.ComId == com_id && b.VIP == floor.VIP && a.IsWorking == false
                          select a);
            if (floor.VIP)
            {
                tables = tables.Where(n => n.FloorId == floorId);
            }
            return tables.ToList();
        }

        public IList<Room> GetTableWorking(int com_id, int momentTableId, int floorId)
        {
            var floorSrv = IoC.Resolve<IFloorService>();
            var floor = floorSrv.GetbyKey(floorId);

            var tables = (from a in Query
                          join b in IoC.Resolve<IFloorService>().Query on a.FloorId equals b.Id
                          where a.Id != momentTableId && a.Status == true && b.Status == true &&
                          a.ComId == com_id && b.VIP == floor.VIP && a.IsWorking == true
                          select a);
            if (floor.VIP)
            {
                tables = tables.Where(n => n.FloorId == floorId);
            }
            return tables.ToList();
        }

        public bool Update(Room room, out string message)
        {
            try
            {
                room.ModifyBy = CurrentUser.Instance.User.UserName;
                room.ModifyDate = DateTime.Now;
                Update(room);
                CommitChange();
                message = hdcore.Utils.TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }
    }
}
