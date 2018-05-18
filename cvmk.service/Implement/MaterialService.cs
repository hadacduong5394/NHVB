using cvmk.context.domain.Material;
using cvmk.service.Interface;
using hdcore;
using hdcore.Utils;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class MaterialService: BaseService<Material, int>, IMaterialService
    {
        private readonly IErrorService log;
        public MaterialService(IDbFactory dbFactory): base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Material entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Id != entity.Id && n.Code.Equals(entity.Code) && n.Status == true))
                {
                    message = "Mã này đã tồn tại.";
                    return false;
                }

                CreateNew(entity);
                CommitChange();
                message = TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            try
            {
                Delete(id);
                CommitChange();
                message = TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Edit(Material entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Id != entity.Id && n.Code.Equals(entity.Code) && n.Status == true))
                {
                    message = "Mã này đã tồn tại.";
                    return false;
                }

                Update(entity);
                CommitChange();
                message = TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public Material GetbyCode(string code, int com_id)
        {
            return Query.FirstOrDefault(n => n.Code == code && n.ComId == com_id);
        }

        public IList<Material> GetbyFilter(int comId, string code, string name, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == comId);
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(n => n.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<Material> GetMaterialForImport(int com_id)
        {
            return Query.Where(n => n.Status == true && n.ComId == com_id).ToList();
        }

        public IList<Material> GetMaterials(int comId, string keyword, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == comId);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(n => n.Code.Contains(keyword) || n.Name.Contains(keyword));
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }
    }
}
