using cvmk.service.Helper;
using cvmk.service.Interface;
using hdcontext.AdminDomain.IdentityCode;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;

namespace cvmk.service.Implement
{
    public class SupplierCodeService : BaseService<SupplierCode, int>, ISupplierCodeService
    {
        private readonly IErrorService log;

        public SupplierCodeService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool GenCode(out string result)
        {
            BeginTran();
            try
            {
                var entity = new SupplierCode()
                {
                    KeyCode = KeyCode.KEY_CODE_SUPPLIER
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