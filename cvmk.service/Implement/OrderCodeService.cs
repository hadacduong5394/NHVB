using cvmk.service.Helper;
using cvmk.service.Interface;
using hdcontext.AdminDomain.IdentityCode;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;

namespace cvmk.service.Implement
{
    public class OrderCodeService : BaseService<OrderCode, int>, IOrderCodeService
    {
        private readonly IErrorService log;

        public OrderCodeService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool GenCode(out string result)
        {
            BeginTran();
            try
            {
                var entity = new OrderCode()
                {
                    KeyCode = KeyCode.KEY_CODE_ORDER
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
                RollbackTran();
                log.TryLog(ex);
                result = ex.Message;
                return false;
            }
        }
    }
}