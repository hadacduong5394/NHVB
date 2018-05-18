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
    public class GroupProductCategoryService : BaseService<GroupProductCategory, int>, IGroupProductCategoryService
    {
        private readonly IErrorService log;
        public GroupProductCategoryService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(GroupProductCategory entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Status == true && n.Name.Equals(entity.Name)))
                {
                    message = "Tên này đã tồn tại trước đó rồi.";
                    return false;
                }
                entity.CreateBy = CurrentUser.Instance.User.UserName;
                entity.CreateDate = DateTime.Now;
                entity.ComId = CurrentUser.Instance.User.ComId;
                CreateNew(entity);
                CommitChange();
                message = "";
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = "Thêm mới nhóm sản phẩm thất bại.";
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            try
            {
                Delete(id);
                CommitChange();
                message = "Xóa thành công.";
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = "Xóa nhóm sản phẩm thất bại.";
                return false;
            }
        }

        public IList<GroupProductCategory> GetbyFilter(int com_id, string name, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }
            query = query.OrderBy(n => n.CreateDate);
            total = query.Count();
            var result = query.Skip(currentPage * pageSize).Take(pageSize).ToList();
            return result;
        }

        public bool Update(GroupProductCategory entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Id != entity.Id && n.Status == true && n.Name.Equals(entity.Name)))
                {
                    message = "Tên này đã tồn tại trước đó rồi.";
                    return false;
                }
                entity.ModifyBy = CurrentUser.Instance.User.UserName;
                entity.ModifyDate = DateTime.Now;
                entity.ComId = CurrentUser.Instance.User.ComId;
                Update(entity);
                CommitChange();
                message = "Cập nhật thành công.";
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = "Cập nhật nhóm sản phẩm thất bại.";
                return false;
            }
        }
    }
}
