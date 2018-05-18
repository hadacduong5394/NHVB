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
    public class CustomerService: BaseService<Customer, int>, ICustomerService
    {
        private readonly IErrorService log;
        public CustomerService(IDbFactory factory): base(factory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Customer entity, out string message)
        {
            try
            {
                if (Query.Any(n => n.Code.Equals(entity.Code)))
                {
                    message = "Mã khách hàng này đã tồn tại.";
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

        public IList<Customer> GetbyFilter(int com_id, string code, string name, string taxcode, string email, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(n => n.Code == code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name == name);
            }
            if (!string.IsNullOrEmpty(taxcode))
            {
                query = query.Where(n => n.TaxCode == taxcode);
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(n => n.Email == email);
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool Update(Customer entity, out string message)
        {
            try
            {
                if (Query.Any(n =>n.Id != entity.Id && n.Code.Equals(entity.Code)))
                {
                    message = "Mã khách hàng này đã tồn tại.";
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
