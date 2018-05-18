using cvmk.service.Interface;
using hdcontext.AdminDomain.Domain;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Linq;
using System.Collections.Generic;
using hdcore;
using cvmk.context.IdentityConfiguration;

namespace cvmk.service.Implement
{
    public class TopMenuService : BaseService<TopMenu, int>, ITopMenuService
    {
        private readonly IErrorService log;
        public TopMenuService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(TopMenu entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.ParentId == entity.ParentId && n.OrderNumber == entity.OrderNumber && n.Status == true))
                {
                    message = "Số thứ tự của menu này đã tồn tại.";
                    return false;
                }

                if (!entity.ParentId.HasValue && Query.Count(n => n.ParentId == null && n.Status == true) > 6)
                {
                    message = "Số menu cha không được vượt quá 6.";
                    return false;
                }

                entity.CreateBy = CurrentUser.Instance.User.UserName;
                entity.CreateDate = DateTime.Now;
                CreateNew(entity);
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
                var entity = GetbyKey(id);
                if (Count(n => n.ParentId == id) > 0)
                {
                    message = "Menu cha này đang tồn tại các menu con, xóa thất bại.";
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

        public IList<TopMenu> GetbyFilter(int parentId, int currentPage, int pageSize, out int total)
        {
            var query = Query;
            if (parentId != -1)
            {
                query = query.Where(n => n.ParentId == parentId);
            }
            query = query.OrderBy(n => n.OrderNumber);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<TopMenu> GetChilds(int parentId)
        {
            return GetMulti(n => n.Status == true && n.ParentId == parentId).OrderBy(n => n.OrderNumber).ToList();
        }

        public IList<TopMenu> GetParents()
        {
            return GetMulti(n => n.Status == true && n.ParentId == null).OrderBy(n => n.OrderNumber).ToList();
        }

        public bool Update(TopMenu entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Id != entity.Id && n.ParentId == entity.ParentId && n.OrderNumber == entity.OrderNumber && n.Status == true))
                {
                    message = "Số thứ tự của menu này đã tồn tại.";
                    return false;
                }

                if (!entity.ParentId.HasValue && Query.Count(n => n.ParentId == null && n.Status == true) > 6)
                {
                    message = "Số menu cha không được vượt quá 6.";
                    return false;
                }

                entity.ModifyBy = CurrentUser.Instance.User.UserName;
                entity.ModifyDate = DateTime.Now;
                Update(entity);
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