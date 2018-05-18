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
    public class FloorService: BaseService<Floor, int>, IFloorService
    {
        private readonly IErrorService log;
        public FloorService(IDbFactory dbFactory): base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Floor floor, out string message)
        {
            try
            {
                floor.CreateBy = CurrentUser.Instance.User.UserName;
                floor.CreateDate = DateTime.Now;
                floor.ComId = CurrentUser.Instance.User.ComId;
                CreateNew(floor);
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
                if (IoC.Resolve<IRoomService>().Query.Any(n => n.FloorId == id))
                {
                    message = "Đã tồn tại phòng/bàn trong tầng này, xóa thất bại.";
                    return false;
                }
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

        public IList<Floor> GetbyFilter(int comId, string name, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == comId);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }

            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool Update(Floor floor, out string message)
        {
            try
            {
                floor.ModifyBy = CurrentUser.Instance.User.UserName;
                floor.ModifyDate = DateTime.Now;
                floor.ComId = CurrentUser.Instance.User.ComId;
                Update(floor);
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
