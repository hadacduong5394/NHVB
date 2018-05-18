using cvmk.service.Interface;
using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;

namespace cvmk.service.Implement
{
    public class ErrorService : BaseService<Error, int>, IErrorService
    {
        public ErrorService(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool TryLog(Exception ex)
        {
            try
            {
                CreateNew(new Error
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Status = false
                });
                CommitChange();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}