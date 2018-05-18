using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
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
    public class SupplierService : BaseService<Supplier, int>, ISupplierService
    {
        private readonly IErrorService log;
        public SupplierService(IDbFactory factory) : base(factory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Supplier entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Code == entity.Code))
                {
                    message = "Mã này đã tồn tại.";
                    return false;
                }

                entity.CreateBy = CurrentUser.Instance.User.UserName;
                entity.CreateDate = DateTime.Now;
                entity.ComId = CurrentUser.Instance.User.ComId;
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

        public Supplier GetbyCode(string code)
        {
            return GetSingleByCondition(n => n.Code.Equals(code));
        }

        public IList<Supplier> GetbyFilter(int com_id, string name, string email, string taxcode, string phonenumber, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(n => n.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(taxcode))
            {
                query = query.Where(n => n.TaxCode.Contains(taxcode));
            }
            if (!string.IsNullOrEmpty(phonenumber))
            {
                query = query.Where(n => n.PhoneNumber.Contains(phonenumber));
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool Update(Supplier entity, out string message)
        {
            try
            {
                if (Query.Any(n =>n.Id != entity.Id && n.Code == entity.Code))
                {
                    message = "Mã này đã tồn tại.";
                    return false;
                }

                entity.ModifyBy = CurrentUser.Instance.User.UserName;
                entity.ModifyDate = DateTime.Now;
                entity.ComId = CurrentUser.Instance.User.ComId;
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
