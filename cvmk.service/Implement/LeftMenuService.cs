using cvmk.service.Interface;
using hdcontext.AdminDomain.Domain;
using hdcore;
using hdcore.Utils;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cvmk.service.Implement
{
    public class LeftMenuService : BaseService<LeftMenu, int>, ILeftMenuService
    {
        public LeftMenuService(IDbFactory dbfactory) : base(dbfactory)
        {
        }

        public bool Create(LeftMenu entity, out string message)
        {
            try
            {
                var flag = Query.Any(m => m.ParentId == entity.ParentId && m.OrderNumber == entity.OrderNumber && m.Status == true);
                if (!flag)
                {
                    CreateNew(entity);
                    CommitChange();
                    message = TextHelper.CREAT_SUCCESSFULL;
                    return true;
                }
                else
                {
                    message = "Số thứ tự hiển thị này đã tồn tại.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
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
                message = TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public IList<LeftMenu> GetbyFilter(int parentId, int currentPage, int pageSize, out int total)
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

        public IList<LeftMenu> GetChilds(int parentId)
        {
            var lRp = IoC.Resolve<ILeftMenuService>();
            return lRp.GetMulti(p => p.ParentId == parentId && p.Status == true).OrderBy(n => n.OrderNumber).ToList();
        }

        public IList<LeftMenu> GetParents()
        {
            var lRp = IoC.Resolve<ILeftMenuService>();
            return lRp.GetMulti(p => p.ParentId == null && p.Status == true).OrderBy(n => n.OrderNumber).ToList();
        }

        public bool Update(LeftMenu entity, out string message)
        {
            try
            {
                var flag = Query.Any(m => m.Id != entity.Id && m.ParentId == entity.ParentId && m.OrderNumber == entity.OrderNumber && m.Status == true);
                if (!flag)
                {
                    Update(entity);
                    CommitChange();
                    message = TextHelper.EDIT_SUCCESSFULL;
                    return true;
                }
                else
                {
                    message = "Số thứ tự hiển thị này đã tồn tại.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }


    }
}