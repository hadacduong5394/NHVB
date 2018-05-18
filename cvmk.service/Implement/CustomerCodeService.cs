using cvmk.service.Helper;
using cvmk.service.Interface;
using hdcontext.AdminDomain.IdentityCode;
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
    public class CustomerCodeService: BaseService<CustomerCode, int>, ICustomerCodeService
    {
        private readonly IErrorService log;
        public CustomerCodeService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool GenCode(out string result)
        {
            BeginTran();
            try
            {
                var entity = new CustomerCode()
                {
                    KeyCode = KeyCode.KEY_CODE_CUSTOMER
                };
                var t = CreateNew(entity);
                CommitChange();

                t.KeyCode = t.KeyCode + "." + t.Id;
                Update(t);
                CommitChange();

                CommitTran();
                result = t.KeyCode;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                result = ex.Message;
                return false;
            }
        }
    }
}
